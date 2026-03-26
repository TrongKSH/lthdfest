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
        <span class="lang-switch__caret" aria-hidden="true"></span>
      </button>

      @if (open()) {
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
      display: inline-flex;
      align-items: center;
      gap: 0.35rem;
      min-width: 60px;
      height: 34px;
      padding: 0 8px;
      border: none;
      border-radius: 5px;
      background: rgba(0, 0, 0, 0.55);
      color: #fff;
      cursor: pointer;
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
    .lang-switch__caret {
      width: 0;
      height: 0;
      border-left: 4px solid transparent;
      border-right: 4px solid transparent;
      border-top: 6px solid #fff;
      opacity: 0.9;
      transition: transform 0.16s ease;
    }
    .lang-switch--open .lang-switch__caret {
      transform: rotate(180deg);
    }
    .lang-switch__menu {
      position: absolute;
      top: calc(100% + 6px);
      right: 0;
      min-width: 84px;
      padding: 3px;
      border: none;
      border-radius: 7px;
      background: rgba(0, 0, 0, 0.9);
      box-shadow: 0 8px 20px rgba(0, 0, 0, 0.34);
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
      background: rgba(255, 255, 255, 0.12);
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
