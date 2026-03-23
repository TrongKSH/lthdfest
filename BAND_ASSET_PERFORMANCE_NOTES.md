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
   - `npm run assets:optimize:bands`
4. Run budget check:
   - `npm run assets:check:bands`
5. If budget check passes, done.
6. If filename/path changed, update URLs in:
   - `backend/FestivalApi/Data/BandSeedData.cs`

## If You Only Have One Source Image and It Fails Budget

You can still pass by making optimization more aggressive in:
- `frontend/scripts/optimize-band-assets.mjs`

Tune these values:
- `HERO_MAX_WIDTH` (example: `1920 -> 1600` or `1400`)
- `LOGO_MAX_WIDTH` (example: `900 -> 700`)
- Hero JPEG quality (example: `74 -> 65`)
- Logo PNG quality (example: `80 -> 65`)

Then run again:
- `npm run assets:optimize:bands`
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

