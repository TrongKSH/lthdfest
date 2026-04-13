import fs from 'node:fs/promises';
import path from 'node:path';
import sharp from 'sharp';

const bandsRoot = path.resolve('public/assets/bands');
const imageExt = new Set(['.jpg', '.jpeg', '.png', '.jfif', '.JPG', '.PNG']);

const HERO_MAX_WIDTH = 1920;
const LOGO_MAX_WIDTH = 900;
/** Non-band folders (announcement, etc.): treat every raster as a wide banner. */
const BANNER_MAX_WIDTH = HERO_MAX_WIDTH;

async function walk(dir) {
  const entries = await fs.readdir(dir, { withFileTypes: true });
  const files = await Promise.all(
    entries.map(async (entry) => {
      const fullPath = path.join(dir, entry.name);
      if (entry.isDirectory()) return walk(fullPath);
      return [fullPath];
    })
  );
  return files.flat();
}

function classifyBand(filePath) {
  const base = path.basename(filePath).toLowerCase();
  if (base.startsWith('hero')) return 'hero';
  if (base.startsWith('logo')) return 'logo';
  return null;
}

/** @param {'hero' | 'logo' | 'banner'} kind */
function targetWidth(kind) {
  if (kind === 'hero') return HERO_MAX_WIDTH;
  if (kind === 'banner') return BANNER_MAX_WIDTH;
  return LOGO_MAX_WIDTH;
}

/** @param {'hero' | 'logo' | 'banner'} kind */
function quality(kind) {
  if (kind === 'logo') return 80;
  return 74;
}

function isUnderBands(rootAbs) {
  const rel = path.relative(bandsRoot, rootAbs);
  return rel === '' || (!rel.startsWith('..') && !path.isAbsolute(rel));
}

/**
 * Band trees: only hero* / logo* (same as before).
 * Other folders: every raster image uses banner preset (hero-like size/quality).
 */
function classifyForRoot(filePath, rootAbs) {
  if (isUnderBands(rootAbs)) {
    return classifyBand(filePath);
  }
  const ext = path.extname(filePath);
  if (!imageExt.has(ext)) return null;
  return 'banner';
}

async function optimize(filePath, kind) {
  const ext = path.extname(filePath);
  if (!imageExt.has(ext)) return false;

  const input = sharp(filePath, { failOn: 'none' });
  const metadata = await input.metadata();

  // Apply EXIF orientation to pixels before resize; otherwise portrait phone JPEGs
  // (landscape pixels + Orientation tag) save as landscape after re-encode.
  const resized = sharp(filePath, { failOn: 'none' })
    .rotate()
    .resize({
      width: targetWidth(kind),
      withoutEnlargement: true,
      fit: 'inside',
    });

  const lowerExt = ext.toLowerCase();
  if (lowerExt === '.png') {
    await resized.png({
      quality: quality(kind),
      compressionLevel: 9,
      palette: true,
      effort: 10,
    }).toFile(filePath + '.tmp');
  } else {
    await resized.jpeg({
      quality: quality(kind),
      mozjpeg: true,
      progressive: true,
    }).toFile(filePath + '.tmp');
  }

  const [before, after] = await Promise.all([fs.stat(filePath), fs.stat(filePath + '.tmp')]);
  if (after.size >= before.size) {
    await fs.unlink(filePath + '.tmp');
    return false;
  }

  await fs.rename(filePath + '.tmp', filePath);
  const beforeKb = Math.round(before.size / 1024);
  const afterKb = Math.round(after.size / 1024);
  const widthInfo = metadata.width ? ` (${metadata.width}px)` : '';
  console.log(`${path.relative(process.cwd(), filePath)}: ${beforeKb}KB -> ${afterKb}KB${widthInfo}`);
  return true;
}

function usage() {
  console.log(`Usage:
  node scripts/optimize-band-assets.mjs [folder]

  With no arguments, optimizes all images under:
    public/assets/bands

  With a folder (relative to the frontend project or absolute), optimizes only that tree:
    node scripts/optimize-band-assets.mjs public/assets/bands/Kinh
    node scripts/optimize-band-assets.mjs public/assets/announcement

  Rules:
  - Under public/assets/bands: only files named hero* and logo* are processed.
  - Any other folder: all JPEG/PNG images in that folder are processed (banner preset).
`);
}

async function main() {
  const raw = process.argv.slice(2);
  if (raw.includes('--help') || raw.includes('-h')) {
    usage();
    return;
  }
  const argv = raw.filter((a) => !a.startsWith('-'));
  const targetRoot = argv.length === 0 ? bandsRoot : path.resolve(process.cwd(), argv[0]);

  try {
    const st = await fs.stat(targetRoot);
    if (!st.isDirectory()) {
      console.error(`Not a directory: ${targetRoot}`);
      process.exit(1);
    }
  } catch {
    console.error(`Path does not exist: ${targetRoot}`);
    usage();
    process.exit(1);
  }

  const files = await walk(targetRoot);
  let changed = 0;

  for (const filePath of files) {
    const kind = classifyForRoot(filePath, targetRoot);
    if (!kind) continue;
    const didChange = await optimize(filePath, kind);
    if (didChange) changed += 1;
  }

  console.log(
    `Optimized ${changed} asset(s) under ${path.relative(process.cwd(), targetRoot) || '.'}.`
  );
}

main().catch((error) => {
  console.error(error);
  process.exit(1);
});
