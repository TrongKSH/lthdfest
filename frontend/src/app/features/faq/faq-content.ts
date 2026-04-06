/**
 * FAQ item ids — copy lives in `assets/i18n/{lang}.json` under `faq.items.<id>`.
 */
export const FAQ_ITEM_IDS = [1, 2, 3, 4, 5, 6] as const;

export type FaqItemId = (typeof FAQ_ITEM_IDS)[number];
