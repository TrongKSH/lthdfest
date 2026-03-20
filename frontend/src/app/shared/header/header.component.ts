import {
  Component,
  ChangeDetectionStrategy,
  effect,
  inject,
  signal,
} from '@angular/core';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';

@Component({
  selector: 'app-header',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [RouterLink, RouterLinkActive],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss',
})
export class HeaderComponent {
  private readonly router = inject(Router);
  protected readonly menuOpen = signal(false);

  constructor() {
    effect((onCleanup) => {
      if (!this.menuOpen()) {
        return;
      }
      const onScroll = (): void => {
        this.closeMenu();
      };
      /** Close when tapping anywhere except hamburger, nav links, or logo */
      const onOutsidePointer = (e: PointerEvent): void => {
        const t = e.target as HTMLElement | null;
        if (!t) return;
        if (t.closest('.menu-btn')) return;
        if (t.closest('.nav-link')) return;
        if (t.closest('.nav')) return;
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

  /** Home + always scroll to top (including when already on home). */
  onLogoClick(event: MouseEvent): void {
    event.preventDefault();
    this.closeMenu();
    void this.router.navigateByUrl('/').finally(() => {
      window.scrollTo({ top: 0, behavior: 'smooth' });
    });
  }
}
