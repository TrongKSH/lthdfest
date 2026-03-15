import { ApplicationConfig, importProvidersFrom, provideBrowserGlobalErrorListeners } from '@angular/core';
import { provideRouter } from '@angular/router';
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
    provideRouter(routes),
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
