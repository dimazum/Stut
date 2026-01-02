import 'zone.js';// for debug in razor
import './styles.css'; //for debug in razor
 
import { bootstrapApplication } from '@angular/platform-browser';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { authInterceptor } from './app/auth.interceptor';
import { AppComponent } from './app/app.component';
import { createCustomElement } from '@angular/elements';
import { RandomTextComponent } from './app/random-text/random-text.component';
import { LanguagePickerComponent } from './app/header/language-picker/language-picker.component';
import { AccountComponent } from './app/header/account/account.component';
import { NavItemComponent } from './app/header/nav-item/nav-item.component';
import { CalendarComponent } from './app/calendar/calendar.component';


bootstrapApplication(AppComponent, {

  providers: [provideHttpClient(withInterceptors([authInterceptor]))],
}).then(appRef => {
  const injector = appRef.injector;

  const textEl = createCustomElement(RandomTextComponent, { injector });
  customElements.define('app-text', textEl);

  const accountEl = createCustomElement(AccountComponent, { injector });
  customElements.define('app-account', accountEl);

  const navItemEl = createCustomElement(NavItemComponent, { injector });
  customElements.define('app-nav-item', navItemEl);

  const calenarEl = createCustomElement(CalendarComponent, { injector });
  customElements.define('stu-calendar', calenarEl);
  
});
