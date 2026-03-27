export type PackArt = 'ticket_main' | 'countdown';

export type TicketPackVariant =
  | 'brotherhood'
  | 'longtranh'
  | 'hodau'
  | 'metalhead'
  | 'vip'
  | 'atdoor';

/** Aligns with `tickets.when.*` translation keys. */
export type TicketWhenKey = 'full' | 'may8' | 'may9';
export type MerchSize = 'XS' | 'S' | 'M' | 'L' | 'XL' | 'XXL';

export interface TicketPackDef {
  id: string;
  variant: TicketPackVariant;
  unitPriceVnd: number;
  buyUrl: string;
  art: PackArt;
  whenKey: TicketWhenKey;
}

export const PRESALE_DEF = {
  id: 'presale' as const,
  unitPriceVnd: 549_000,
  buyUrl: '#' as const,
};

export const TICKET_PACK_DEFS: readonly TicketPackDef[] = [
  {
    id: 'brotherhood',
    variant: 'brotherhood',
    unitPriceVnd: 999_000,
    buyUrl: '#',
    art: 'ticket_main',
    whenKey: 'full',
  },
  {
    id: 'longtranh',
    variant: 'longtranh',
    unitPriceVnd: 349_000,
    buyUrl: '#',
    art: 'countdown',
    whenKey: 'may8',
  },
  {
    id: 'hodau',
    variant: 'hodau',
    unitPriceVnd: 349_000,
    buyUrl: '#',
    art: 'countdown',
    whenKey: 'may9',
  },
  {
    id: 'metalhead',
    variant: 'metalhead',
    unitPriceVnd: 849_000,
    buyUrl: '#',
    art: 'ticket_main',
    whenKey: 'full',
  },
  {
    id: 'vip',
    variant: 'vip',
    unitPriceVnd: 1_099_000,
    buyUrl: '#',
    art: 'ticket_main',
    whenKey: 'full',
  },
  {
    id: 'atdoor',
    variant: 'atdoor',
    unitPriceVnd: 649_000,
    buyUrl: '#',
    art: 'countdown',
    whenKey: 'full',
  },
] as const;

export const MERCH_SIZE_OPTIONS: readonly MerchSize[] = ['XS', 'S', 'M', 'L', 'XL', 'XXL'] as const;

export function getTicketPricing(purchaseKey: string): {
  unitPriceVnd: number;
  summaryKey: string;
} | null {
  if (purchaseKey === 'presale') {
    return {
      unitPriceVnd: PRESALE_DEF.unitPriceVnd,
      summaryKey: 'tickets.packs.presale.summaryDisplayName',
    };
  }
  const pack = TICKET_PACK_DEFS.find((p) => p.id === purchaseKey);
  if (!pack) return null;
  return {
    unitPriceVnd: pack.unitPriceVnd,
    summaryKey: `tickets.packs.${pack.id}.summaryDisplayName`,
  };
}

export function requiresMerchSize(purchaseType: string): boolean {
  return purchaseType === 'metalhead' || purchaseType === 'vip';
}

/** Translation key under `tickets.packs.*` for tier line in purchase header. */
export function purchaseTierTitleKey(purchaseKey: string): string | null {
  if (purchaseKey === 'presale') return 'tickets.packs.presale.title';
  const pack = TICKET_PACK_DEFS.find((p) => p.id === purchaseKey);
  if (!pack) return null;
  return `tickets.packs.${pack.id}.peekTitle`;
}

export function getPurchaseHeaderMetaKeys(purchaseKey: string): { whenKey: string; whereKey: string } {
  if (purchaseKey === 'presale') {
    return { whenKey: 'tickets.when.full', whereKey: 'tickets.meta.where' };
  }
  const pack = TICKET_PACK_DEFS.find((p) => p.id === purchaseKey);
  if (!pack) {
    return { whenKey: 'tickets.when.full', whereKey: 'tickets.meta.where' };
  }
  return {
    whenKey: `tickets.when.${pack.whenKey}`,
    whereKey: 'tickets.meta.where',
  };
}

/** Perks group key under `tickets.perks.<key>`. */
export function perksGroupKey(purchaseType: string): string {
  if (purchaseType === 'presale') return 'presale';
  if (
    purchaseType === 'longtranh' ||
    purchaseType === 'hodau' ||
    purchaseType === 'brotherhood' ||
    purchaseType === 'metalhead' ||
    purchaseType === 'vip'
  ) {
    return purchaseType;
  }
  return 'default';
}
