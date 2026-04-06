import { Injectable, inject, DOCUMENT } from '@angular/core';
import { TranslocoService } from '@ngneat/transloco';

const STORAGE_KEY = 'lthd.lang';

export type AppLang = 'en' | 'vi';

@Injectable({ providedIn: 'root' })
export class AppLocaleService {
  private readonly doc = inject(DOCUMENT);
  private readonly transloco = inject(TranslocoService);

  get activeLang(): AppLang {
    const l = this.transloco.getActiveLang();
    return l === 'vi' ? 'vi' : 'en';
  }

  setLang(lang: AppLang): void {
    this.transloco.setActiveLang(lang);
    if (typeof localStorage !== 'undefined') {
      localStorage.setItem(STORAGE_KEY, lang);
    }
    this.applyDocumentLang(lang);
  }

  applyDocumentLang(lang: AppLang): void {
    this.doc.documentElement.lang = lang;
  }

  toggleLang(): void {
    this.setLang(this.activeLang === 'vi' ? 'en' : 'vi');
  }

  /** VND display aligned with active language. */
  formatVnd(amount: number): string {
    const locale = this.activeLang === 'vi' ? 'vi-VN' : 'en-US';
    const suffix = this.activeLang === 'vi' ? 'vnđ' : 'VND';
    return `${amount.toLocaleString(locale)} ${suffix}`;
  }
}
