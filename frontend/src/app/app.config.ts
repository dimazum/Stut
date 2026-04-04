import { APP_INITIALIZER, ApplicationConfig, importProvidersFrom } from '@angular/core';
import { provideHttpClient } from '@angular/common/http';
import { provideAnimations } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';
import { initLocalization, LocalizationService } from './services/localization-service';
import { TranslateModule } from '@ngx-translate/core';

export const appConfig: ApplicationConfig = {
  providers: [
    provideHttpClient(),
    provideAnimations(),
    importProvidersFrom(
      TranslateModule.forRoot({
        fallbackLang: 'en',
      }),
      ToastrModule.forRoot(),
      ToastrModule.forRoot({
        positionClass: 'toast-bottom-right',
        timeOut: 3000,
        closeButton: true,
        progressBar: true,
      })
    ),
    {
      provide: APP_INITIALIZER,
      multi: true,
      deps: [LocalizationService],
      useFactory: initLocalization,
    },
  ],
};
