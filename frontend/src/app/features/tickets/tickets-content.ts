/** Shared festival copy for ticket cards */
export const FESTIVAL_EVENT_NAME = 'Long Tranh Hổ Đấu Festival';
export const FESTIVAL_WHEN = 'Thời gian: T6-T7, 08-09/05/2026';
export const FESTIVAL_WHERE =
  'Địa điểm: 327 Trần Xuân Soạn, Tân Kiểng, Quận 7 (cũ), TP.HCM';

/** Static tickets page copy — replace buyUrl when sales go live */
export const PRESALE = {
  title: 'PRE-SALE',
  eventName: FESTIVAL_EVENT_NAME,
  when: FESTIVAL_WHEN,
  where: FESTIVAL_WHERE,
  priceLabel: 'Giá:',
  priceAmount: '549.000 vnđ',
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
    when: FESTIVAL_WHEN,
    where: FESTIVAL_WHERE,
    priceLabel: 'Giá:',
    priceAmount: '799.000 vnđ',
    buyUrl: '#',
    buyLabel: 'MUA VÉ NGAY',
    art: 'countdown',
  },
  {
    id: 'hodau',
    variant: 'hodau',
    peekTitle: 'HỔ ĐẦU',
    hoverTitle: 'HỔ ĐẦU',
    eventName: FESTIVAL_EVENT_NAME,
    when: FESTIVAL_WHEN,
    where: FESTIVAL_WHERE,
    priceLabel: 'Giá:',
    priceAmount: '799.000 vnđ',
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
    priceAmount: '999.000 vnđ',
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
    priceAmount: '1.299.000 vnđ',
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
    priceAmount: '699.000 vnđ',
    buyUrl: '#',
    buyLabel: 'MUA VÉ NGAY',
    art: 'countdown',
  },
];
