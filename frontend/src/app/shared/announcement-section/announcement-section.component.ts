import { Component, ChangeDetectionStrategy, computed, inject } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { startWith } from 'rxjs';
import { TranslocoPipe, TranslocoService } from '@ngneat/transloco';

@Component({
  selector: 'app-announcement-section',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [TranslocoPipe],
  templateUrl: './announcement-section.component.html',
  styleUrl: './announcement-section.component.scss',
})
export class AnnouncementSectionComponent {
  /** Official announcement post (Facebook). */
  readonly facebookPostUrl = 'https://www.facebook.com/share/p/14gg9mnZ1n9/' as const;

  private readonly transloco = inject(TranslocoService);

  /** Re-compute paragraph list when language changes. */
  private readonly lang = toSignal(
    this.transloco.langChanges$.pipe(startWith(this.transloco.getActiveLang())),
    { initialValue: this.transloco.getActiveLang() },
  );

  readonly paragraphs = computed(() => {
    this.lang();
    const raw = this.transloco.translateObject('announcement.paragraphs') as unknown;
    return Array.isArray(raw) ? (raw as string[]) : [];
  });
}
