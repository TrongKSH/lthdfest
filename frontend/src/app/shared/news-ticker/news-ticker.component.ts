import { Component, ChangeDetectionStrategy } from '@angular/core';

@Component({
  selector: 'app-news-ticker',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './news-ticker.component.html',
  styleUrl: './news-ticker.component.scss',
})
export class NewsTickerComponent {
  protected readonly items = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];
  protected readonly itemsAlt = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];
}
