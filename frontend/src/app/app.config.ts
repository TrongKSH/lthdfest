import {
  APP_INITIALIZER,
  ApplicationConfig,
  importProvidersFrom,
  isDevMode,
  provideBrowserGlobalErrorListeners,
} from '@angular/core';
import { PreloadAllModules, provideRouter, withInMemoryScrolling, withPreloading } from '@angular/router';
import { provideHttpClient } from '@angular/common/http';
import { provideTransloco, TranslocoService } from '@ngneat/transloco';
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

import { initAppLanguage } from './i18n/lang-init';
import { AppLocaleService } from './i18n/locale.service';
import { TranslocoHttpLoader } from './i18n/transloco-http.loader';
import { routes } from './app.routes';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideRouter(
      routes,
      withPreloading(PreloadAllModules),
      withInMemoryScrolling({
        // Restore scroll on back/forward; still scroll to top on forward navigations.
        scrollPositionRestoration: 'enabled',
        anchorScrolling: 'enabled',
      })
    ),
    provideHttpClient(),
    provideTransloco({
      config: {
        availableLangs: ['en', 'vi'],
        defaultLang: 'vi',
        fallbackLang: 'en',
        reRenderOnLangChange: true,
        missingHandler: {
          useFallbackTranslation: true,
        },
        prodMode: !isDevMode(),
      },
      loader: TranslocoHttpLoader,
    }),
    {
      provide: APP_INITIALIZER,
      useFactory: initAppLanguage,
      deps: [TranslocoService, AppLocaleService],
      multi: true,
    },
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
