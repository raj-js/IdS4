import { Component, OnInit, Optional, Inject } from '@angular/core';
import { SocialService, DA_SERVICE_TOKEN, ITokenService } from '@delon/auth';
import { Router } from '@angular/router';
import { ReuseTabService } from '@delon/abc';
import { StartupService } from '@core';
import { OidcSecurityService } from 'angular-auth-oidc-client';

@Component({
	selector: 'app-social-login',
	templateUrl: './social-login.component.html',
	styles: [],
	providers: [ SocialService ]
})
export class SocialLoginComponent implements OnInit {
	constructor(
		private oidcSecurityService: OidcSecurityService,
		private router: Router,
		@Optional()
		@Inject(ReuseTabService)
		@Inject(DA_SERVICE_TOKEN)
		private tokenService: ITokenService,
		private startupSrv: StartupService
	) {}

	ngOnInit() {
		this.oidcSecurityService.getIsAuthorized().subscribe((isAuth) => {
			if (isAuth) {
				this.startupSrv.load().then(() => {
					let url = this.tokenService.referrer.url || '/';
					if (url.includes('/passport')) url = '/';
					this.router.navigateByUrl(url);
				});
			} else {
				this.oidcSecurityService.authorize((url) => (window.location.href = url));
			}
		});
	}
}
