import { Injectable } from '@angular/core';
import { OidcSecurityService } from 'angular-auth-oidc-client';
import { Observable, Subject, from } from 'rxjs';
import { Router } from '@angular/router';

@Injectable()
export class OidcService {
	isAuthorized: boolean;
	userData: any;
	security: OidcSecurityService;

	constructor(private oidcSecurityService: OidcSecurityService, private router: Router) {
		this.security = oidcSecurityService;
		if (this.oidcSecurityService.moduleSetup) {
			this.doCallbackIfRequired();
		} else {
			this.oidcSecurityService.onModuleSetup.subscribe(() => {
				this.doCallbackIfRequired();
			});
		}

		this.oidcSecurityService.getIsAuthorized().subscribe((auth) => {
			this.isAuthorized = auth;
		});

		this.oidcSecurityService.getUserData().subscribe((d) => {
			console.log(d);
			this.userData = d;
		});
	}

	login(): void {
		this.oidcSecurityService.authorize((url) => {
			window.location.href = url;
		});
	}

	getToken(): string | null {
		const token = this.isAuthorized ? this.oidcSecurityService.getToken() : null;
		console.log(token);
		return token;
	}

	logout() {
		this.oidcSecurityService.logoff((url) => {
			window.location.href = url;
		});
	}

	private doCallbackIfRequired() {
		this.oidcSecurityService.authorizedCallbackWithCode(window.location.toString());
	}
}
