import {
  Component,
  ChangeDetectionStrategy,
  effect,
  inject,
  signal,
} from '@angular/core';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { TranslocoPipe } from '@ngneat/transloco';

type DesktopNavSection = 'soul' | 'commerce' | 'info';

@Component({
  selector: 'app-header',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [RouterLink, RouterLinkActive, TranslocoPipe],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss',
})
export class HeaderComponent {
  private readonly router = inject(Router);
  protected readonly menuOpen = signal(false);
  /** At most one desktop flyout open — tracks which top-level group is active. */
  protected readonly desktopOpenSubmenu = signal<DesktopNavSection | null>(null);
  /**
   * After a navigation click, force-hide that group’s submenu until pointer leaves it
   * (then opening works again).
   */
  protected readonly suppressedDesktopSubmenu = signal<DesktopNavSection | null>(null);

  constructor() {
    effect((onCleanup) => {
      if (!this.menuOpen()) {
        return;
      }
      const isMobileViewport = window.matchMedia('(max-width: 1024px)').matches;
      const onScroll = (): void => {
        if (isMobileViewport) return;
        this.closeMenu();
      };
      /** Close when tapping anywhere except hamburger, nav links, or logo */
      const onOutsidePointer = (e: PointerEvent): void => {
        const t = e.target as HTMLElement | null;
        if (!t) return;
        if (t.closest('.menu-btn')) return;
        if (t.closest('.app-lang-float')) return;
        if (t.closest('.nav-link')) return;
        if (t.closest('.nav')) return;
        if (t.closest('.mobile-drawer')) return;
        if (t.closest('.logo-link')) return;
        this.closeMenu();
      };
      window.addEventListener('scroll', onScroll, { passive: true });
      document.addEventListener('pointerdown', onOutsidePointer, true);
      onCleanup(() => {
        window.removeEventListener('scroll', onScroll);
        document.removeEventListener('pointerdown', onOutsidePointer, true);
      });
    });
  }

  toggleMenu(): void {
    this.menuOpen.update((v) => !v);
  }

  closeMenu(): void {
    this.menuOpen.set(false);
  }

  /** Call when a desktop primary/sub nav action runs (route change or in-page jump). */
  onDesktopMenuNavigate(section: DesktopNavSection): void {
    this.closeMenu();
    this.desktopOpenSubmenu.set(null);
    this.suppressedDesktopSubmenu.set(section);
  }

  protected isDesktopSubmenuOpen(section: DesktopNavSection): boolean {
    return (
      this.desktopOpenSubmenu() === section &&
      this.suppressedDesktopSubmenu() !== section
    );
  }

  onDesktopNavItemEnter(section: DesktopNavSection): void {
    this.desktopOpenSubmenu.set(section);
  }

  onDesktopNavItemLeave(section: DesktopNavSection): void {
    if (this.suppressedDesktopSubmenu() === section) {
      this.suppressedDesktopSubmenu.set(null);
    }
    if (this.desktopOpenSubmenu() === section) {
      this.desktopOpenSubmenu.set(null);
    }
  }

  onDesktopNavItemFocusOut(event: FocusEvent, section: DesktopNavSection): void {
    const host = event.currentTarget;
    const next = event.relatedTarget as Node | null;
    if (host instanceof HTMLElement && next && host.contains(next)) {
      return;
    }
    this.onDesktopNavItemLeave(section);
  }

  /** Leaving the whole desktop nav strip — close any flyout and clear suppress state. */
  onDesktopNavBarLeave(): void {
    this.desktopOpenSubmenu.set(null);
    this.suppressedDesktopSubmenu.set(null);
  }

  /** Home + always scroll to top (including when already on home). */
  onLogoClick(event: MouseEvent): void {
    event.preventDefault();
    this.closeMenu();
    void this.router.navigateByUrl('/').finally(() => {
      window.scrollTo({ top: 0, behavior: 'smooth' });
    });
  }
}
