import { Component, ChangeDetectionStrategy, inject, signal } from '@angular/core';
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
