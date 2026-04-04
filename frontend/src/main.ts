import 'zone.js';// for debug in razor
import './styles.css'; //for debug in razor
import './stu-utils.js';

import { bootstrapApplication } from '@angular/platform-browser';
import { AppComponent } from './app/app.component';
import { createCustomElement } from '@angular/elements';
import { RandomTextComponent } from './app/random-text/random-text.component';
import { AccountComponent } from './app/header/account/account.component';
import { NavItemComponent } from './app/header/nav-item/nav-item.component';
import { CalendarComponent } from './app/calendar/calendar.component';
import { SubHeaderComponent } from './app/header/sub-header/sub-header.component';
import { WarmUpComponent } from './app/warm-up/warm-up.component';
import { HistogramComponent } from './app/histogram/histogram.component';
import { ConfirmEmailComponent } from './app/email/confirm-email/confirm-email.component';
import { ResetPasswordComponent } from './app/email/reset-password/reset-password.component';
import { appConfig } from './app/app.config';

bootstrapApplication(AppComponent, appConfig).then(appRef => {
  const injector = appRef.injector;

  const textEl = createCustomElement(RandomTextComponent, { injector });
  customElements.define('app-text', textEl);

  const accountEl = createCustomElement(AccountComponent, { injector });
  customElements.define('app-account', accountEl);

  const navItemEl = createCustomElement(NavItemComponent, { injector });
  customElements.define('app-nav-item', navItemEl);

  const calenarEl = createCustomElement(CalendarComponent, { injector });
  customElements.define('stu-calendar', calenarEl);

  const subHeaderEl = createCustomElement(SubHeaderComponent, { injector });
  customElements.define('stu-sub-header', subHeaderEl);

  const warmUpEl = createCustomElement(WarmUpComponent, { injector });
  customElements.define('stu-warm-up', warmUpEl);

  const histogramEl = createCustomElement(HistogramComponent, { injector });
  customElements.define('stu-histogram', histogramEl);

  const confirmEmailEl = createCustomElement(ConfirmEmailComponent, { injector });
  customElements.define('stu-confirm-email', confirmEmailEl);

  const resetPasswordEl = createCustomElement(ResetPasswordComponent, { injector });
  customElements.define('stu-reset-password', resetPasswordEl);
});
