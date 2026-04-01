import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./features/home/home.component').then((m) => m.HomeComponent),
  },
  {
    path: 'lineup',
    loadComponent: () =>
      import('./features/lineup/lineup.component').then((m) => m.LineupComponent),
    data: { preload: true },
  },
  {
    path: 'bands/:id',
    loadComponent: () =>
      import('./features/band-detail/band-detail.component').then((m) => m.BandDetailComponent),
  },
  {
    path: 'faq',
    loadComponent: () =>
      import('./features/faq/faq.component').then((m) => m.FaqComponent),
  },
  {
    path: 'tickets',
    loadComponent: () =>
      import('./features/tickets/tickets.component').then((m) => m.TicketsComponent),
    data: { preload: true },
  },
];
