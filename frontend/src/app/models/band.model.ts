export type LineupDay = 'LongTranh' | 'HoDau';

/** Slim shape returned by the band list endpoint (no bio fields). */
export interface BandListItem {
  id: number;
  name: string;
  heroUrl?: string;
  logoUrl?: string;
  isFeaturedOnHome?: boolean;
  isSecret?: boolean;
  lineupDay: LineupDay;
  lineupPosition: number;
}

/** Full band detail returned by the single-band endpoint. */
export interface Band extends BandListItem {
  bio: string;
  /** Optional English bio from API; falls back to `bio` when empty. */
  bioEn?: string;
}

export interface LineupBand {
  id: number;
  name: string;
  logoUrl?: string;
  isSecret?: boolean;
  lineupDay: LineupDay;
  lineupPosition: number;
}
