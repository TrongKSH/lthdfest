import {
  Component,
  ChangeDetectionStrategy,
  input,
  viewChild,
  effect,
  signal,
  AfterViewInit,
  ElementRef,
} from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-hero-section',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [RouterLink],
  templateUrl: './hero-section.component.html',
  styleUrl: './hero-section.component.scss',
})
export class HeroSectionComponent implements AfterViewInit {
  /** Optional hero image URL (from API or default asset). */
  imageUrl = input<string | undefined>();
  /** Optional event name for accessibility. */
  eventName = input<string>('');

  private readonly heroVideoRef = viewChild<ElementRef<HTMLVideoElement>>('heroVideo');
  /** Shown on mobile/tablet when autoplay is blocked so user can tap to play. */
  readonly showPlayPrompt = signal(false);

  constructor() {
    effect(() => {
      const video = this.heroVideoRef()?.nativeElement;
      if (!video) return;
      const onCanPlay = () => this.tryPlay(video);
      video.addEventListener('loadeddata', onCanPlay, { once: true });
      video.addEventListener('canplay', onCanPlay, { once: true });
      return () => {
        video.removeEventListener('loadeddata', onCanPlay);
        video.removeEventListener('canplay', onCanPlay);
      };
    });
  }

  ngAfterViewInit(): void {
    const video = this.heroVideoRef()?.nativeElement;
    if (video) this.tryPlay(video);
  }

  /** Try to start playback (muted). If blocked, show tap-to-play on touch devices. */
  tryPlay(video: HTMLVideoElement): void {
    if (!video) return;
    video.muted = true;
    video.setAttribute('playsinline', '');
    video.setAttribute('webkit-playsinline', '');
    const p = video.play();
    if (typeof p !== 'undefined' && p !== null && typeof p.then === 'function') {
      p.catch(() => {
        if (this.isTouchDevice()) this.showPlayPrompt.set(true);
      });
    }
    // If still paused after a short delay (e.g. iOS not ready yet), show prompt on touch
    setTimeout(() => {
      if (video.paused && this.isTouchDevice()) this.showPlayPrompt.set(true);
    }, 500);
  }

  private isTouchDevice(): boolean {
    return (
      typeof window !== 'undefined' &&
      ('ontouchstart' in window || navigator.maxTouchPoints > 0)
    );
  }

  /** User tapped to play (from overlay). */
  onTapToPlay(): void {
    const video = this.heroVideoRef()?.nativeElement;
    if (video) {
      video.muted = true;
      video.play().then(() => this.showPlayPrompt.set(false)).catch(() => {});
    }
  }

  /** Called when video has loaded metadata; ensure it stays muted. */
  onVideoLoaded(): void {
    this.ensureMuted(this.heroVideoRef()?.nativeElement ?? null);
  }

  /** Called when video starts playing (hides tap-to-play overlay). */
  onVideoPlay(): void {
    this.showPlayPrompt.set(false);
    this.ensureMuted(this.heroVideoRef()?.nativeElement ?? null);
  }

  /** Keep video playing without sound (enforce muted). */
  ensureMuted(video: HTMLVideoElement | null): void {
    if (video && !video.muted) {
      video.muted = true;
    }
  }
}
