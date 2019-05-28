import { Component, OnInit, Optional, Inject } from '@angular/core';
import { Router } from '@angular/router';
import { ReuseTabService } from '@delon/abc';
import { DA_SERVICE_TOKEN, ITokenService } from '@delon/auth';
import { StartupService } from '@core';
import { _HttpClient } from '@delon/theme';
import { NzMessageService } from 'ng-zorro-antd';
import { OidcSecurityService } from 'angular-auth-oidc-client';

@Component({
	selector: 'app-callback',
	template: ``
})
export class CallbackComponent implements OnInit {
	constructor(
		private oidcSecurityService: OidcSecurityService,
		private router: Router,
		@Optional()
		@Inject(ReuseTabService)
		private reuseTabService: ReuseTabService,
		@Inject(DA_SERVICE_TOKEN) private tokenService: ITokenService,
		private startupSrv: StartupService,
		public http: _HttpClient,
		public msg: NzMessageService
	) {}

	ngOnInit(): void {
		this.oidcSecurityService.getIsAuthorized().subscribe((isAuth) => {
			console.log(`callback is auth ${isAuth}`);
			if (isAuth) {
				this.reuseTabService.clear();
				const token = this.oidcSecurityService.getToken();
				this.tokenService.set({ token });
				this.startupSrv.load().then(() => {
					let url = this.tokenService.referrer.url || '/';
					if (url.includes('/passport')) url = '/';
					this.router.navigateByUrl(url);
				});
			}
		});
	}
}
