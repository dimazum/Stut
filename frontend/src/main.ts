import 'zone.js'; // for debug in razor
import './styles.css'; //for debug in razor

import { bootstrapApplication } from '@angular/platform-browser';
import { AppComponent } from './app/app.component';
import { createCustomElement } from '@angular/elements';
import { RandomTextComponent } from './app/random-text/random-text.component';
import { AccountComponent } from './app/header/account/account.component';
import { NavItemComponent } from './app/header/nav-item/nav-item.component';
import { CalendarComponent } from './app/calendar/calendar.component';
import { ConfirmEmailComponent } from './app/confirm-email/confirm-email.component';
import { appConfig } from './app/app.config';
import { ResetPasswordComponent } from './app/reset-password/reset-password.component';

bootstrapApplication(AppComponent, appConfig).then(appRef => {
  const injector = appRef.injector;

  const textEl = createCustomElement(RandomTextComponent, { injector });
  customElements.define('app-text', textEl);

  const accountEl = createCustomElement(AccountComponent, { injector });
  customElements.define('app-account', accountEl);

  const navItemEl = createCustomElement(NavItemComponent, { injector });
  customElements.define('app-nav-item', navItemEl);

  const calendarEl = createCustomElement(CalendarComponent, { injector });
  customElements.define('stu-calendar', calendarEl);

  const confirmEmailEl = createCustomElement(ConfirmEmailComponent, { injector });
  customElements.define('stu-confirm-email', confirmEmailEl);

  const resetPasswordEl = createCustomElement(ResetPasswordComponent, { injector });
  customElements.define('stu-reset-password', resetPasswordEl);
});
