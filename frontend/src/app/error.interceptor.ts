import { HttpRequest, HttpHandlerFn, HttpEvent, HttpErrorResponse } from '@angular/common/http';
import { Observable, catchError, throwError } from 'rxjs';
import { inject } from '@angular/core';
import { NotificationService } from './services/notification.service';

export const errorInterceptor: (req: HttpRequest<unknown>, next: HttpHandlerFn) => Observable<HttpEvent<unknown>> = 
  (req, next) => {
    const ns = inject(NotificationService); 
    return next(req).pipe(
      catchError((error: HttpErrorResponse) => {
        let message = 'Произошла ошибка';
        console.log(message);

        console.log(ns);
        
         if (error.status !== 401){
             ns.showError(message);
          } 
             
        return throwError(() => error);
      })
    );
  };