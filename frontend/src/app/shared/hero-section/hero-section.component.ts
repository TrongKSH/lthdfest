import { Component, ChangeDetectionStrategy, input } from '@angular/core';

@Component({
  selector: 'app-hero-section',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './hero-section.component.html',
  styleUrl: './hero-section.component.scss',
})
export class HeroSectionComponent {
  /** Optional hero image URL (from API or default asset). */
  imageUrl = input<string | undefined>();
  /** Optional event name for accessibility. */
  eventName = input<string>('');

  scrollTo(event: Event, fragment: string): void {
    event.preventDefault();
    const el = document.getElementById(fragment);
    el?.scrollIntoView({ behavior: 'smooth' });
  }
}
