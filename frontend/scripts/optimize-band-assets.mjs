import fs from 'node:fs/promises';
import path from 'node:path';
import sharp from 'sharp';

const bandsRoot = path.resolve('public/assets/bands');
const imageExt = new Set(['.jpg', '.jpeg', '.png', '.jfif', '.JPG', '.PNG']);

const HERO_MAX_WIDTH = 1920;
const LOGO_MAX_WIDTH = 900;

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

function classify(filePath) {
  const base = path.basename(filePath).toLowerCase();
  if (base.startsWith('hero')) return 'hero';
  if (base.startsWith('logo')) return 'logo';
  return null;
}

function targetWidth(kind) {
  return kind === 'hero' ? HERO_MAX_WIDTH : LOGO_MAX_WIDTH;
}

function quality(kind) {
  return kind === 'hero' ? 74 : 80;
}

async function optimize(filePath, kind) {
  const ext = path.extname(filePath);
  if (!imageExt.has(ext)) return false;

  const image = sharp(filePath, { failOn: 'none' });
  const metadata = await image.metadata();

  const resized = image.resize({
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

async function main() {
  const files = await walk(bandsRoot);
  const targets = files.filter((filePath) => classify(filePath));
  let changed = 0;

  for (const filePath of targets) {
    const kind = classify(filePath);
    if (!kind) continue;
    const didChange = await optimize(filePath, kind);
    if (didChange) changed += 1;
  }

  console.log(`Optimized ${changed} asset(s).`);
}

main().catch((error) => {
  console.error(error);
  process.exit(1);
});
