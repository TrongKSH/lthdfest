import { getBrowserLang, TranslocoService } from '@ngneat/transloco';

import { AppLocaleService } from './locale.service';

const STORAGE_KEY = 'lthd.lang';

function normalizeLang(value: string | null | undefined): 'en' | 'vi' | null {
  if (value === 'en' || value === 'vi') return value;
  return null;
}

function detectInitialLang(): 'en' | 'vi' {
  const stored = normalizeLang(
    typeof localStorage !== 'undefined' ? localStorage.getItem(STORAGE_KEY) : null,
  );
  if (stored) return stored;

  const browser = getBrowserLang();
  if (browser?.toLowerCase().startsWith('vi')) return 'vi';
  return 'en';
}

/** Restore saved or browser language; syncs `html[lang]`. */
export function initAppLanguage(
  transloco: TranslocoService,
  locale: AppLocaleService,
): () => void {
  return () => {
    const lang = detectInitialLang();
    transloco.setActiveLang(lang);
    locale.applyDocumentLang(lang);
  };
}
