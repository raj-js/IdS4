import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { Injectable, Injector } from '@angular/core';
import { OidcSecurityService } from 'angular-auth-oidc-client';
import { Observable } from 'rxjs';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
	private oidcSecurityService: OidcSecurityService;

	constructor(private injector: Injector) {}

	intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
		let requestToForward = req;

		if (this.oidcSecurityService === undefined) {
			this.oidcSecurityService = this.injector.get(OidcSecurityService);
		}

		if (this.oidcSecurityService !== undefined) {
			const token = this.oidcSecurityService.getToken();
			if (token !== '') {
				requestToForward = req.clone({ setHeaders: { Authorization: `Bearer ${token}` } });
			}
		} else {
			console.log('OidcSecurityService undefined: No auth header!');
		}

		return next.handle(requestToForward);
	}
}
