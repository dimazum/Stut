import { HttpRequest, HttpHandlerFn, HttpEvent, HttpErrorResponse } from '@angular/common/http';
import { Observable, catchError, throwError } from 'rxjs';
import { inject } from '@angular/core';
import { NotificationService } from './services/notification.service';

type ErrorSeverity = 'error' | 'warning';

function getSeverityByCode(code: number): ErrorSeverity {
  const warningCodes = [4, 6]; 
  return warningCodes.includes(code) ? 'warning' : 'error';
}

export const errorInterceptor = (
  req: HttpRequest<unknown>,
  next: HttpHandlerFn
): Observable<HttpEvent<unknown>> => {

  const ns = inject(NotificationService);

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {

      const serverError = error?.error;
      const message = serverError?.message ?? 'Unexpected error';
      const code = serverError?.code ?? 0;

      const severity = getSeverityByCode(code);

      if (severity === 'warning') {
        ns.showWarning(message);
      } else {
        ns.showError(message);
      }

      return throwError(() => error);
    })
  );
};