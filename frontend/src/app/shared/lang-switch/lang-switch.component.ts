import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { TranslocoPipe } from '@ngneat/transloco';

import { AppLocaleService, type AppLang } from '../../i18n/locale.service';

@Component({
  selector: 'app-lang-switch',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [TranslocoPipe],
  template: `
    <div class="lang-switch" role="group" [attr.aria-label]="'common.lang.switchTo' | transloco">
      <button
        type="button"
        class="lang-switch__btn"
        [class.lang-switch__btn--active]="locale.activeLang === 'vi'"
        (click)="set('vi')"
      >
        VI
      </button>
      <span class="lang-switch__sep" aria-hidden="true">|</span>
      <button
        type="button"
        class="lang-switch__btn"
        [class.lang-switch__btn--active]="locale.activeLang === 'en'"
        (click)="set('en')"
      >
        EN
      </button>
    </div>
  `,
  styles: `
    :host {
      display: block;
    }
    .lang-switch {
      display: flex;
      align-items: center;
      gap: 0.35rem;
      font-size: 0.75rem;
      font-weight: 700;
      letter-spacing: 0.06em;
      text-transform: uppercase;
    }
    .lang-switch__btn {
      margin: 0;
      padding: 0.2rem 0.35rem;
      border: none;
      background: transparent;
      color: inherit;
      cursor: pointer;
      opacity: 0.55;
      font: inherit;
    }
    .lang-switch__btn:hover,
    .lang-switch__btn:focus-visible {
      opacity: 0.9;
    }
    .lang-switch__btn--active {
      opacity: 1;
      text-decoration: underline;
      text-underline-offset: 0.15em;
    }
    .lang-switch__sep {
      opacity: 0.35;
      user-select: none;
    }
  `,
})
export class LangSwitchComponent {
  protected readonly locale = inject(AppLocaleService);

  set(lang: AppLang): void {
    this.locale.setLang(lang);
  }
}
