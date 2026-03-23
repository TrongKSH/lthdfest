/** Shared festival copy for ticket cards */
export const FESTIVAL_EVENT_NAME = 'Long Tranh Hổ Đấu Festival';
export const FESTIVAL_WHEN = 'T6-T7, 08-09/05/2026';
export const FESTIVAL_WHERE = '327 Trần Xuân Soạn, Tân Kiểng, Quận 7 (cũ), TP.HCM';

/** Static tickets page copy — replace buyUrl when sales go live */
export const PRESALE = {
  title: 'PRE-SALE',
  eventName: FESTIVAL_EVENT_NAME,
  when: FESTIVAL_WHEN,
  where: FESTIVAL_WHERE,
  priceLabel: 'Giá:',
  priceAmount: '549.000 vnđ',
  unitPriceVnd: 549_000,
  /** Checkout / confirmation row label */
  summaryDisplayName: 'Pre-Sale',
  buyUrl: '#',
  buyLabel: 'MUA VÉ NGAY',
} as const;

export type PackArt = 'ticket_main' | 'countdown';

export interface TicketPack {
  id: string;
  variant: 'brotherhood' | 'longtranh' | 'hodau' | 'metalhead' | 'vip' | 'atdoor';
  /** Idle: main line (centered) */
  peekTitle: string;
  peekSub?: string;
  /** Hover: heading lines */
  hoverTitle: string;
  hoverSubtitle?: string;
  eventName: string;
  when: string;
  where: string;
  priceLabel: string;
  priceAmount: string;
  unitPriceVnd: number;
  /** Checkout / confirmation row label */
  summaryDisplayName: string;
  buyUrl: string;
  buyLabel: string;
  /** Idle background */
  art: PackArt;
}

export const TICKET_PACKS: readonly TicketPack[] = [
  {
    id: 'brotherhood',
    variant: 'brotherhood',
    peekTitle: 'GÓI HUYNH ĐỆ',
    peekSub: '(BROTHERHOOD)',
    hoverTitle: 'GÓI HUYNH ĐỆ',
    hoverSubtitle: '(BROTHERHOOD)',
    eventName: FESTIVAL_EVENT_NAME,
    when: FESTIVAL_WHEN,
    where: FESTIVAL_WHERE,
    priceLabel: 'Giá:',
    priceAmount: '999.000 vnđ',
    unitPriceVnd: 999_000,
    summaryDisplayName: 'Gói Huynh Đệ',
    buyUrl: '#',
    buyLabel: 'MUA VÉ NGAY',
    art: 'ticket_main',
  },
  {
    id: 'longtranh',
    variant: 'longtranh',
    peekTitle: 'LONG TRANH',
    hoverTitle: 'LONG TRANH',
    eventName: FESTIVAL_EVENT_NAME,
    when: 'T6, 08/05/2026',
    where: FESTIVAL_WHERE,
    priceLabel: 'Giá:',
    priceAmount: '349.000 vnđ',
    unitPriceVnd: 349_000,
    summaryDisplayName: 'Long Tranh',
    buyUrl: '#',
    buyLabel: 'MUA VÉ NGAY',
    art: 'countdown',
  },
  {
    id: 'hodau',
    variant: 'hodau',
    peekTitle: 'HỔ ĐẤU',
    hoverTitle: 'HỔ ĐẤU',
    eventName: FESTIVAL_EVENT_NAME,
    when: 'T7, 09/05/2026',
    where: FESTIVAL_WHERE,
    priceLabel: 'Giá:',
    priceAmount: '349.000 vnđ',
    unitPriceVnd: 349_000,
    summaryDisplayName: 'Hổ Đấu',
    buyUrl: '#',
    buyLabel: 'MUA VÉ NGAY',
    art: 'countdown',
  },
  {
    id: 'metalhead',
    variant: 'metalhead',
    peekTitle: 'TRUE METALHEAD PACK',
    hoverTitle: 'TRUE METALHEAD PACK',
    eventName: FESTIVAL_EVENT_NAME,
    when: FESTIVAL_WHEN,
    where: FESTIVAL_WHERE,
    priceLabel: 'Giá:',
    priceAmount: '849.000 vnđ',
    unitPriceVnd: 849_000,
    summaryDisplayName: 'True Metalhead Pack',
    buyUrl: '#',
    buyLabel: 'MUA VÉ NGAY',
    art: 'ticket_main',
  },
  {
    id: 'vip',
    variant: 'vip',
    peekTitle: 'VIP',
    hoverTitle: 'VIP',
    eventName: FESTIVAL_EVENT_NAME,
    when: FESTIVAL_WHEN,
    where: FESTIVAL_WHERE,
    priceLabel: 'Giá:',
    priceAmount: '1.099.000 vnđ',
    unitPriceVnd: 1_099_000,
    summaryDisplayName: 'VIP',
    buyUrl: '#',
    buyLabel: 'MUA VÉ NGAY',
    art: 'ticket_main',
  },
  {
    id: 'atdoor',
    variant: 'atdoor',
    peekTitle: 'AT DOOR',
    hoverTitle: 'AT DOOR',
    eventName: FESTIVAL_EVENT_NAME,
    when: FESTIVAL_WHEN,
    where: FESTIVAL_WHERE,
    priceLabel: 'Giá:',
    priceAmount: '649.000 vnđ',
    unitPriceVnd: 649_000,
    summaryDisplayName: 'At Door',
    buyUrl: '#',
    buyLabel: 'MUA VÉ NGAY',
    art: 'countdown',
  },
];

/** Unit price + summary label for checkout (presale or pack id). */
export function getTicketPricing(purchaseKey: string): {
  unitPriceVnd: number;
  summaryDisplayName: string;
} | null {
  if (purchaseKey === 'presale') {
    return {
      unitPriceVnd: PRESALE.unitPriceVnd,
      summaryDisplayName: PRESALE.summaryDisplayName,
    };
  }
  const pack = TICKET_PACKS.find((p) => p.id === purchaseKey);
  if (!pack) return null;
  return {
    unitPriceVnd: pack.unitPriceVnd,
    summaryDisplayName: pack.summaryDisplayName,
  };
}

/** Stable code for Google Sheet / Drive filename (column E + segment 2). */
export function getTicketCodeForPurchaseType(purchaseKey: string): string | null {
  if (purchaseKey === 'presale') return 'LTHD-GA2D';
  const codesByPackId: Record<string, string> = {
    brotherhood: 'LTHD-CMB',
    longtranh: 'LTHD-GA1',
    hodau: 'LTHD-GA2',
    metalhead: 'LTHD-TMH',
    vip: 'LTHD-VIP',
    atdoor: 'LTHD-ATDOOR',
  };
  return codesByPackId[purchaseKey] ?? null;
}

/** Page header line: "Festival - TIER" */
export function getPurchaseHeaderTitle(purchaseKey: string): string {
  if (purchaseKey === 'presale') {
    return `${FESTIVAL_EVENT_NAME} - ${PRESALE.title}`;
  }
  const pack = TICKET_PACKS.find((p) => p.id === purchaseKey);
  if (!pack) return FESTIVAL_EVENT_NAME;
  return `${FESTIVAL_EVENT_NAME} - ${pack.peekTitle}`;
}

/** Header detail lines for purchase flow. */
export function getPurchaseHeaderMeta(purchaseKey: string): { when: string; where: string } {
  if (purchaseKey === 'presale') {
    return { when: PRESALE.when, where: PRESALE.where };
  }
  const pack = TICKET_PACKS.find((p) => p.id === purchaseKey);
  if (!pack) {
    return { when: FESTIVAL_WHEN, where: FESTIVAL_WHERE };
  }
  return { when: pack.when, where: pack.where };
}
