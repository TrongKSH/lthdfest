import { ApplicationConfig, importProvidersFrom, provideBrowserGlobalErrorListeners } from '@angular/core';
import { PreloadAllModules, provideRouter, withInMemoryScrolling, withPreloading } from '@angular/router';
import { provideHttpClient } from '@angular/common/http';
import {
  LucideAngularModule,
  Facebook,
  Instagram,
  Youtube,
  Music2,
  Mail,
  Phone,
  CircleQuestionMark,
} from 'lucide-angular';

import { routes } from './app.routes';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideRouter(
      routes,
      withPreloading(PreloadAllModules),
      withInMemoryScrolling({
        scrollPositionRestoration: 'top',
      })
    ),
    provideHttpClient(),
    importProvidersFrom(
      LucideAngularModule.pick({
        Facebook,
        Instagram,
        Youtube,
        Music2,
        Mail,
        Phone,
        CircleQuestionMark,
      })
    ),
  ],
};
