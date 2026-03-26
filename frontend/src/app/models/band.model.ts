export type LineupDay = 'LongTranh' | 'HoDau';

export interface Band {
  id: number;
  name: string;
  bio: string;
  /** Optional English bio from API; falls back to `bio` when empty. */
  bioEn?: string;
  heroUrl?: string;
  logoUrl?: string;
  isFeaturedOnHome?: boolean;
  isSecret?: boolean;
  lineupDay: LineupDay;
  lineupPosition: number;
}

export interface LineupBand {
  id: number;
  name: string;
  logoUrl?: string;
  isSecret?: boolean;
  lineupDay: LineupDay;
  lineupPosition: number;
}
