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
      import('./features/home/home.component').then((m) => m.HomeComponent),
  },
  {
    path: 'bands/:id',
    loadComponent: () =>
      import('./features/band-detail/band-detail.component').then((m) => m.BandDetailComponent),
  },
];
