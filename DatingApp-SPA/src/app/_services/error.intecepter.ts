import { Injectable } from '@angular/core';
import {
  HttpInterceptor,
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpErrorResponse,
  HTTP_INTERCEPTORS,
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
        catchError(error => {
           if (error.status === 401){ // Unauthorized error from server using Status Code
                return throwError(error.statusText);
           }
           if (error instanceof HttpErrorResponse){
             // Internal server error from Application-Error using Headers
             const applicationError = error.headers.get('Application-Error');
             if (applicationError){
                 return throwError(applicationError);
             }

             const serverError = error.error;
             let modalStateError = '';
             // All the application Errors thrown by server
             if (serverError.errors && typeof serverError.errors === 'object'){
                for (const key in serverError.errors){
                    if (serverError.errors[key]){
                       modalStateError += serverError.errors[key] + '\n';
                    }
                }
             }
             return throwError(modalStateError || error.error || 'Server Error');
           }
        })
    );

  }
}

export const ErrorInterceptorProvider = {
    provide: HTTP_INTERCEPTORS,
    useClass: ErrorInterceptor,
    multi: true
}
