# Band Asset Performance Notes

This document explains how to update band logos/heroes without breaking performance on:
- `lineup` page
- band detail page (`/bands/:id`)

## Current Asset Rules

- Hero files (`hero*`) budget: **<= 600KB**
- Logo files (`logo*`) budget: **<= 250KB**
- Keep naming stable when possible:
  - `hero.jpg` / `hero.jpeg` / `hero.png`
  - `logo.png` / `logo.jpg`

## Standard Update Workflow (when replacing or adding images)

1. Put new file in:
   - `frontend/public/assets/bands/<band-name>/`
2. Keep same filename if possible (`hero*`, `logo*`) to avoid backend URL edits.
3. Run optimizer:
   - `cd frontend`
   - All bands: `npm run assets:optimize:bands` (or `npm run assets:optimize`)
   - One band only: `npm run assets:optimize -- public/assets/bands/<band-name>`
4. Run budget check:
   - `npm run assets:check:bands`
5. If budget check passes, done.
6. If filename/path changed, update URLs in:
   - `backend/FestivalApi/Data/BandSeedData.cs`

## Optimizing a chosen folder (one band or non-band assets)

The optimizer script is `frontend/scripts/optimize-band-assets.mjs`. With **no arguments** it processes every band under `public/assets/bands`. With a **folder path** (relative to `frontend`, or absolute), it only walks that directory tree.

Run from `frontend`:

```bash
npm run assets:optimize -- public/assets/bands/<band-name>
```

Examples:

```bash
npm run assets:optimize -- public/assets/bands/1818
npm run assets:optimize -- public/assets/announcement
```

**Rules:**

- Paths **under** `public/assets/bands`: same as the full run — only files whose names start with `hero` or `logo` are optimized.
- **Any other** folder (e.g. `public/assets/announcement`): every JPEG/PNG in that tree is optimized using the same “wide banner” settings as heroes (max width 1920, hero-style quality).

Usage summary:

```bash
node scripts/optimize-band-assets.mjs --help
```

The budget check script (`npm run assets:check:bands`) still validates **all** bands under `public/assets/bands`; run it after changes even when you only optimized one band folder.

## If You Only Have One Source Image and It Fails Budget

You can still pass by making optimization more aggressive in:
- `frontend/scripts/optimize-band-assets.mjs`

Tune these values:
- `HERO_MAX_WIDTH` (example: `1920 -> 1600` or `1400`)
- `LOGO_MAX_WIDTH` (example: `900 -> 700`)
- Hero JPEG quality (example: `74 -> 65`)
- Logo PNG quality (example: `80 -> 65`)

Then run again (entire bands tree or a single folder — see [Optimizing a chosen folder](#optimizing-a-chosen-folder-one-band-or-non-band-assets)):
- `npm run assets:optimize:bands` and/or `npm run assets:optimize -- public/assets/bands/<band-name>`
- `npm run assets:check:bands`

## Quality vs Budget Tradeoff Guidance

- Prefer hero images as JPEG/WebP style content (photos).
- Use PNG for logos only when transparency is required.
- If image quality becomes unacceptable:
  - Use a per-file exception strategy (recommended) instead of raising all global budgets.
  - Keep exceptions minimal and documented.

## Performance Features Already Implemented

- Lineup image loading hints in `lineup.component.html`:
  - lazy loading below fold
  - async decode
  - width/height attributes
  - fetch priority tuning for top items
- Service-level caching in `band.service.ts`:
  - cached list endpoints via `shareReplay(1)`
  - detail lookup from cache first, then API fallback
- Router preloading enabled in `app.config.ts`:
  - `withPreloading(PreloadAllModules)`
- Lightweight lineup API endpoint:
  - `GET /api/bands/lineup`
  - returns only fields needed for lineup cards

## Quick Verification Checklist

After updating assets, check:

1. `npm run assets:check:bands` passes.
2. Open `/lineup` and verify logos look correct.
3. Click a logo and verify detail hero quality/loading.
4. Navigate back and open another band detail; ensure transitions feel smooth.
5. Optional: run Lighthouse/network check for LCP and image bytes.

