import { Component, OnInit, Optional, Inject } from '@angular/core';
import { OidcService } from '@shared/services/oidc.service';
import { Router } from '@angular/router';
import { ReuseTabService } from '@delon/abc';
import { DA_SERVICE_TOKEN, ITokenService } from '@delon/auth';
import { StartupService } from '@core';
import { _HttpClient } from '@delon/theme';
import { NzMessageService } from 'ng-zorro-antd';

@Component({
	selector: 'app-callback',
	template: ``
})
export class CallbackComponent implements OnInit {
	constructor(
		private oidcService: OidcService,
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
		this.oidcService.security.getIsAuthorized().subscribe((auth) => {
			if (auth) {
				this.reuseTabService.clear();
				const token = this.oidcService.getToken();
				this.tokenService.set({
					token
				});
				this.startupSrv.load().then(() => {
					let url = this.tokenService.referrer.url || '/';
					if (url.includes('/passport')) url = '/';
					this.router.navigateByUrl(url);
				});
			}
		});
	}
}
