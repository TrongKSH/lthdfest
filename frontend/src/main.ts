import { registerLocaleData } from '@angular/common';
import localeEn from '@angular/common/locales/en';
import localeVi from '@angular/common/locales/vi';
import { bootstrapApplication } from '@angular/platform-browser';

import { appConfig } from './app/app.config';
import { App } from './app/app';

registerLocaleData(localeEn, 'en');
registerLocaleData(localeVi, 'vi');

bootstrapApplication(App, appConfig)
  .catch((err) => console.error(err));
