import { Component, ChangeDetectionStrategy, inject } from '@angular/core';
import { HeroSectionComponent } from '../../shared/hero-section/hero-section.component';
import { NewsTickerComponent } from '../../shared/news-ticker/news-ticker.component';
import { BandsSectionComponent } from '../../shared/bands-section/bands-section.component';
import { CountdownSectionComponent } from '../../shared/countdown-section/countdown-section.component';
import { FooterComponent } from '../../shared/footer/footer.component';
import { FestivalService } from '../../services/festival.service';
import { toSignal } from '@angular/core/rxjs-interop';

@Component({
  selector: 'app-home',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [
    HeroSectionComponent,
    NewsTickerComponent,
    BandsSectionComponent,
    CountdownSectionComponent,
    FooterComponent,
  ],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss',
})
export class HomeComponent {
  private readonly festivalService = inject(FestivalService);
  readonly festival = toSignal(this.festivalService.getFestival(), { initialValue: null });
}
