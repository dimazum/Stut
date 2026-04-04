import { Component } from '@angular/core';
import { FooterComponent } from './footer/footer.component';
import { HttpClientModule } from '@angular/common/http';
import { ReactiveFormsModule } from '@angular/forms';
import { NotificationComponent } from './notification-component/notification-component.component';

@Component({
  // eslint-disable-next-line @angular-eslint/component-selector
  selector: 'app-root',
  standalone: true,
  imports: [
    FooterComponent,
    NotificationComponent,
    HttpClientModule,
    ReactiveFormsModule
],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent {}
