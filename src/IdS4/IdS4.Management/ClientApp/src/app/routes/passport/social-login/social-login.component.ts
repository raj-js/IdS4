import { Component, OnInit, Optional, Inject } from '@angular/core';
import { SocialService, DA_SERVICE_TOKEN, ITokenService } from '@delon/auth';
import { OidcService } from '@shared/services/oidc.service';
import { Router } from '@angular/router';
import { ReuseTabService } from '@delon/abc';
import { StartupService } from '@core';

@Component({
	selector: 'app-social-login',
	templateUrl: './social-login.component.html',
	styles: [],
	providers: [ SocialService ]
})
export class SocialLoginComponent implements OnInit {
	constructor(
		private oidcService: OidcService,
		private router: Router,
		@Optional()
		@Inject(ReuseTabService)
		private reuseTabService: ReuseTabService,
		@Inject(DA_SERVICE_TOKEN) private tokenService: ITokenService,
		private startupSrv: StartupService
	) {}

	ngOnInit() {
		if (this.oidcService.security.moduleSetup) {
			this.oidcService.login();
		} else {
			this.oidcService.security.onModuleSetup.subscribe(() => {
				this.oidcService.login();
			});
		}
	}

	private doCallbackIfRequired() {
		if (!this.oidcService.isAuthorized) {
			this.oidcService.login();
		} else {
			this.startupSrv.load().then(() => {
				let url = this.tokenService.referrer.url || '/';
				if (url.includes('/passport')) url = '/';
				this.router.navigateByUrl(url);
			});
		}
	}
}
