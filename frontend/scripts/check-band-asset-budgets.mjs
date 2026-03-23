import fs from 'node:fs/promises';
import path from 'node:path';

const bandsRoot = path.resolve('public/assets/bands');
const imageExt = new Set(['.jpg', '.jpeg', '.png', '.jfif', '.JPG', '.PNG']);

const HERO_MAX_BYTES = 600 * 1024;
const LOGO_MAX_BYTES = 250 * 1024;

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

async function main() {
  const files = await walk(bandsRoot);
  const violations = [];

  for (const filePath of files) {
    if (!imageExt.has(path.extname(filePath))) continue;
    const kind = classify(filePath);
    if (!kind) continue;

    const stat = await fs.stat(filePath);
    const limit = kind === 'hero' ? HERO_MAX_BYTES : LOGO_MAX_BYTES;
    if (stat.size > limit) {
      violations.push({
        file: path.relative(process.cwd(), filePath),
        sizeKb: Math.round(stat.size / 1024),
        limitKb: Math.round(limit / 1024),
      });
    }
  }

  if (!violations.length) {
    console.log('All band asset budgets are within limits.');
    return;
  }

  console.error('Asset budget violations found:');
  for (const violation of violations) {
    console.error(`- ${violation.file}: ${violation.sizeKb}KB (limit ${violation.limitKb}KB)`);
  }
  process.exit(1);
}

main().catch((error) => {
  console.error(error);
  process.exit(1);
});
