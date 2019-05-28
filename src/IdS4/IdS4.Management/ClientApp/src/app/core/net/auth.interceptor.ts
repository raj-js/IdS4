import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { Injectable, Injector } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
	// private authService: AuthService;

	constructor(private injector: Injector) {}

	intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
		let requestToForward = req;

		// if (this.authService === undefined) {
		// 	this.authService = this.injector.get(AuthService);
		// }

		// if (this.authService !== undefined) {
		// 	const token = this.authService.getToken();
		// 	if (token !== '') {
		// 		requestToForward = req.clone({ setHeaders: { Authorization: `Bearer ${token}` } });
		// 	}
		// } else {
		// 	console.log('OidcSecurityService undefined: No auth header!');
		// }

		return next.handle(requestToForward);
	}
}
