import 'zone.js';// for debug in razor
import './styles.css'; //for debug in razor
 
import { Injector } from '@angular/core';
import { bootstrapApplication } from '@angular/platform-browser';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { authInterceptor } from './app/auth.interceptor';
import { AppComponent } from './app/app.component';
import { createCustomElement } from '@angular/elements';
import { RandomTextComponent } from './app/random-text/random-text.component';
import { LanguagePickerComponent } from './app/header/language-picker/language-picker.component';
import { AccountComponent } from './app/header/account/account.component';
import { provideRouter } from '@angular/router';
import { routes } from './app/app.routes';
import { MainContentComponent } from './app/main-content/main-content.component';
import { NavItemComponent } from './app/header/nav-item/nav-item.component';


bootstrapApplication(AppComponent, {

  providers: [provideHttpClient(withInterceptors([authInterceptor])),
  provideRouter(routes),],
}).then(appRef => {
  const injector = appRef.injector;

  // const textEl = createCustomElement(RandomTextComponent, { injector });
  // customElements.define('app-text', textEl);

  const lpEl = createCustomElement(LanguagePickerComponent, { injector });
  customElements.define('language-picker', lpEl);

  const accountEl = createCustomElement(AccountComponent, { injector });
  customElements.define('app-account', accountEl);

  const mainContentEl = createCustomElement(MainContentComponent, { injector });
  customElements.define('angular-content', mainContentEl);

  const navItemEl = createCustomElement(NavItemComponent, { injector });
  customElements.define('app-nav-item', navItemEl);

  
});
