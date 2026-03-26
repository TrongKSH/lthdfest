import {
  ChangeDetectionStrategy,
  Component,
  DestroyRef,
  computed,
  effect,
  inject,
  signal,
  untracked,
} from '@angular/core';
import { takeUntilDestroyed, toSignal } from '@angular/core/rxjs-interop';
import { Location } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { fromEvent } from 'rxjs';
import { debounceTime, switchMap } from 'rxjs/operators';
import { startWith } from 'rxjs';
import { TranslocoPipe, TranslocoService } from '@ngneat/transloco';

import { BandService } from '../../services/band.service';

@Component({
  selector: 'app-band-detail',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [TranslocoPipe],
  templateUrl: './band-detail.component.html',
  styleUrl: './band-detail.component.scss',
})
export class BandDetailComponent {
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly location = inject(Location);
  private readonly bandService = inject(BandService);
  private readonly destroyRef = inject(DestroyRef);
  private readonly transloco = inject(TranslocoService);
  private readonly activeLang = toSignal(
    this.transloco.langChanges$.pipe(startWith(this.transloco.getActiveLang())),
    { initialValue: this.transloco.getActiveLang() },
  );
  private heroImgEl: HTMLImageElement | null = null;

  /** True when object-fit: cover crops the image vertically — enable slow pan. */
  protected readonly heroPanVertical = signal(false);

  readonly band = toSignal<import('../../models/band.model').Band | null | undefined>(
    this.route.paramMap.pipe(
      switchMap((params) => {
        const id = params.get('id');
        return this.bandService.getBand(id ? +id : 0);
      })
    ),
    { initialValue: undefined }
  );

  protected readonly isLoading = computed(() => this.band() === undefined);
  protected readonly isNotFound = computed(() => this.band() === null);

  constructor() {
    effect(() => {
      this.band();
      untracked(() => {
        this.heroImgEl = null;
        this.heroPanVertical.set(false);
      });
    });

    fromEvent(window, 'resize')
      .pipe(debounceTime(150), takeUntilDestroyed(this.destroyRef))
      .subscribe(() => {
        const el = this.heroImgEl;
        if (el?.isConnected) {
          this.updateHeroPanVertical(el);
        }
      });
  }

  protected onHeroImgLoad(event: Event): void {
    const img = event.target as HTMLImageElement;
    this.heroImgEl = img;
    this.updateHeroPanVertical(img);
  }

  /**
   * Under object-fit: cover, vertical pan only helps when the scaled image
   * height exceeds the frame height.
   */
  private updateHeroPanVertical(img: HTMLImageElement): void {
    const nw = img.naturalWidth;
    const nh = img.naturalHeight;
    if (!nw || !nh) {
      this.heroPanVertical.set(false);
      return;
    }
    const { width: cw, height: ch } = img.getBoundingClientRect();
    if (ch < 1) {
      this.heroPanVertical.set(false);
      return;
    }
    const scale = Math.max(cw / nw, ch / nh);
    const renderedHeight = nh * scale;
    this.heroPanVertical.set(renderedHeight > ch + 0.5);
  }

  protected readonly bioParagraphs = computed(() => {
    this.activeLang();
    const b = this.band();
    const preferEn = this.transloco.getActiveLang() === 'en';
    const raw = (preferEn && b?.bioEn?.trim() ? b.bioEn : b?.bio)?.trim() ?? '';
    if (!raw) return [];
    return raw.split(/\n{2,}/).map((part) => part.trim()).filter(Boolean);
  });

  /** Prefer real history (lineup vs home); fall back to home #bands when there is no in-app entry. */
  protected goBack(): void {
    if (typeof history !== 'undefined' && history.length > 1) {
      this.location.back();
    } else {
      void this.router.navigate(['/'], { fragment: 'bands' });
    }
  }
}
