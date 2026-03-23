export type LineupDay = 'LongTranh' | 'HoDau';

export interface Band {
  id: number;
  name: string;
  bio: string;
  heroUrl?: string;
  logoUrl?: string;
  lineupDay: LineupDay;
  lineupPosition: number;
}
