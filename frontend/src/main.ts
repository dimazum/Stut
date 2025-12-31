import 'zone.js';// for debug in razor
 
import { Injector } from '@angular/core';
import { bootstrapApplication } from '@angular/platform-browser';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { authInterceptor } from './app/auth.interceptor';
import { AppComponent } from './app/app.component';
import { createCustomElement } from '@angular/elements';
import { RandomTextComponent } from './app/random-text/random-text.component';


bootstrapApplication(AppComponent, {
  providers: [provideHttpClient(withInterceptors([authInterceptor]))],
}).then(appRef => {
  const injector = appRef.injector;
  const el = createCustomElement(RandomTextComponent, { injector });
  customElements.define('app-text', el);
});
