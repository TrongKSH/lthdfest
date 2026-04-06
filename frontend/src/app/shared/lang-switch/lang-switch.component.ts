import { ChangeDetectionStrategy, Component, ElementRef, HostListener, inject, signal } from '@angular/core';
import { TranslocoPipe } from '@ngneat/transloco';

import { AppLocaleService, type AppLang } from '../../i18n/locale.service';

@Component({
  selector: 'app-lang-switch',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [TranslocoPipe],
  template: `
    <div class="lang-switch" [class.lang-switch--open]="open()" [attr.aria-label]="'common.lang.switchTo' | transloco">
      <button
        type="button"
        class="lang-switch__trigger"
        [attr.aria-expanded]="open()"
        [attr.aria-label]="'common.lang.switchTo' | transloco"
        (click)="toggle()"
      >
        <span
          class="lang-switch__flag"
          [class.lang-switch__flag--vi]="locale.activeLang === 'vi'"
          [class.lang-switch__flag--en]="locale.activeLang === 'en'"
          aria-hidden="true"
        ></span>
      </button>

      @if (open()) {
        <button type="button" class="lang-switch__overlay" aria-hidden="true" tabindex="-1" (click)="open.set(false)"></button>
        <div class="lang-switch__menu" role="menu">
          <button type="button" class="lang-switch__item" role="menuitemradio" [attr.aria-checked]="locale.activeLang === 'vi'" (click)="set('vi')">
            <span class="lang-switch__flag lang-switch__flag--vi" aria-hidden="true"></span>
            <span>VI</span>
          </button>
          <button type="button" class="lang-switch__item" role="menuitemradio" [attr.aria-checked]="locale.activeLang === 'en'" (click)="set('en')">
            <span class="lang-switch__flag lang-switch__flag--en" aria-hidden="true"></span>
            <span>EN</span>
          </button>
        </div>
      }
    </div>
  `,
  styles: `
    :host {
      display: block;
    }
    .lang-switch {
      position: relative;
      display: inline-flex;
      align-items: center;
      justify-content: center;
    }
    .lang-switch__trigger {
      position: relative;
      z-index: 52;
      display: inline-flex;
      align-items: center;
      gap: 0;
      min-width: 44px;
      height: 30px;
      padding: 0 4px;
      border: none;
      border-radius: 5px;
      background: transparent;
      box-shadow: none;
      backdrop-filter: none;
      -webkit-backdrop-filter: none;
      color: #fff;
      cursor: pointer;
    }
    .lang-switch__trigger:hover,
    .lang-switch__trigger:focus-visible {
      background: transparent;
    }
    .lang-switch__overlay {
      position: fixed;
      inset: 0;
      border: 0;
      padding: 0;
      margin: 0;
      background: rgba(0, 0, 0, 0.12);
      z-index: 49;
      cursor: default;
    }
    .lang-switch__flag {
      width: 22px;
      height: 22px;
      border-radius: 50%;
      display: inline-flex;
      align-items: center;
      justify-content: center;
      font-size: 0.68rem;
      font-weight: 800;
      border: none;
      box-sizing: border-box;
    }
    .lang-switch__flag--vi {
      background: #da251d;
      color: #f9da49;
      line-height: 1;
    }
    .lang-switch__flag--vi::before {
      content: '★';
      transform: translateY(-0.5px);
    }
    .lang-switch__flag--en {
      background-image: url('/assets/images/flag-en.png');
      background-size: cover;
      background-position: center;
      background-repeat: no-repeat;
      color: transparent;
    }
    .lang-switch__flag--en::before {
      content: '';
    }
    .lang-switch__menu {
      position: absolute;
      top: calc(100% + 6px);
      right: 0;
      min-width: 84px;
      padding: 3px;
      border: none;
      border-radius: 7px;
      background: rgba(0, 0, 0, 0.56);
      box-shadow: 0 8px 20px rgba(0, 0, 0, 0.24);
      z-index: 50;
    }
    .lang-switch__item {
      width: 100%;
      display: flex;
      align-items: center;
      gap: 6px;
      padding: 5px 7px;
      border: 0;
      border-radius: 5px;
      background: transparent;
      color: #fff;
      font-size: 0.68rem;
      font-weight: 700;
      letter-spacing: 0.05em;
      cursor: pointer;
    }
    .lang-switch__item:hover,
    .lang-switch__item:focus-visible {
      background: transparent;
    }
  `,
})
export class LangSwitchComponent {
  protected readonly locale = inject(AppLocaleService);
  protected readonly open = signal(false);
  private readonly host = inject(ElementRef<HTMLElement>);

  @HostListener('document:pointerdown', ['$event'])
  onDocumentPointerDown(event: PointerEvent): void {
    const target = event.target as Node | null;
    if (!target) return;
    if (this.host.nativeElement.contains(target)) return;
    this.open.set(false);
  }

  toggle(): void {
    this.open.update((v) => !v);
  }

  set(lang: AppLang): void {
    this.locale.setLang(lang);
    this.open.set(false);
  }
}
