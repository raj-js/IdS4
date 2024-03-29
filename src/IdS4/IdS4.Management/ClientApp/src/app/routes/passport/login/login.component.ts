import { _HttpClient } from '@delon/theme';
import { Component, OnDestroy, Inject, Optional } from '@angular/core';
import { Router } from '@angular/router';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { NzMessageService, NzModalService } from 'ng-zorro-antd';
import { SocialService, ITokenService, DA_SERVICE_TOKEN } from '@delon/auth';
import { ReuseTabService } from '@delon/abc';
import { StartupService } from '@core';
import { ConfigurationService } from '@shared/services/configuration.service';
import { StorageService } from '@shared/services/storage.service';
import { OidcService } from '@shared/services/oidc.service';

@Component({
	selector: 'passport-login',
	templateUrl: './login.component.html',
	styleUrls: [ './login.component.less' ],
	providers: [ SocialService ]
})
export class UserLoginComponent implements OnDestroy {
	form: FormGroup;
	error = '';
	type = 0;
	count = 0;
	interval$: any;
	coreApiUrl: string;

	constructor(
		fb: FormBuilder,
		modalSrv: NzModalService,
		private router: Router,
		@Optional()
		@Inject(ReuseTabService)
		private reuseTabService: ReuseTabService,
		@Inject(DA_SERVICE_TOKEN) private tokenService: ITokenService,
		private startupSrv: StartupService,
		public http: _HttpClient,
		public msg: NzMessageService,
		private configurationService: ConfigurationService,
		private storageService: StorageService,
		private oidcService: OidcService
	) {
		this.configurationService.settingsLoaded$.subscribe((_) => {
			this.coreApiUrl = this.configurationService.serverSettings.coreApiUrl;
			this.storageService.store('CoreApiUrl', this.coreApiUrl);
		});

		this.form = fb.group({
			userName: [ null, [ Validators.required, Validators.minLength(4) ] ],
			password: [ null, Validators.required ],
			mobile: [ null, [ Validators.required, Validators.pattern(/^1\d{10}$/) ] ],
			captcha: [ null, [ Validators.required ] ],
			remember: [ true ]
		});
		modalSrv.closeAll();
	}

	// #region fields
	get userName() {
		return this.form.controls.userName;
	}
	get password() {
		return this.form.controls.password;
	}
	get mobile() {
		return this.form.controls.mobile;
	}
	get captcha() {
		return this.form.controls.captcha;
	}

	// #endregion

	switch(ret: any) {
		this.type = ret.index;
	}

	// #region get captcha

	getCaptcha() {
		if (this.mobile.invalid) {
			this.mobile.markAsDirty({ onlySelf: true });
			this.mobile.updateValueAndValidity({ onlySelf: true });
			return;
		}
		this.count = 59;
		this.interval$ = setInterval(() => {
			this.count -= 1;
			if (this.count <= 0) clearInterval(this.interval$);
		}, 1000);
	}

	// #endregion

	submit() {
		//this.error = '';
		//if (this.type === 0) {
		//	this.userName.markAsDirty();
		//	this.userName.updateValueAndValidity();
		//	this.password.markAsDirty();
		//	this.password.updateValueAndValidity();
		//	if (this.userName.invalid || this.password.invalid) return;
		//} else {
		//	this.mobile.markAsDirty();
		//	this.mobile.updateValueAndValidity();
		//	this.captcha.markAsDirty();
		//	this.captcha.updateValueAndValidity();
		//	if (this.mobile.invalid || this.captcha.invalid) return;
		//}
		//// 默认配置中对所有HTTP请求都会强制 [校验](https://ng-alain.com/auth/getting-started) 用户 Token
		//// 然一般来说登录请求不需要校验，因此可以在请求URL加上：`/login?_allow_anonymous=true` 表示不触发用户 Token 校验
		//this.http
		//	.post(`${this.coreApiUrl}/api/identity/authorize?_allow_anonymous=true`, {
		//		type: this.type,
		//		userName: this.userName.value,
		//		password: this.password.value
		//	})
		//	.subscribe((res: any) => {
		//		if (res.msg !== 'ok') {
		//			this.error = res.msg;
		//			return;
		//		}
		//		// 清空路由复用信息
		//		this.reuseTabService.clear();
		//		// 设置用户Token信息
		//		this.tokenService.set(res.user);
		//		// 重新获取 StartupService 内容，我们始终认为应用信息一般都会受当前用户授权范围而影响
		//		this.startupSrv.load().then(() => {
		//			let url = this.tokenService.referrer!.url || '/';
		//			if (url.includes('/passport')) url = '/';
		//			this.router.navigateByUrl(url);
		//		});
		//	});
	}

	ngOnDestroy(): void {
		if (this.interval$) clearInterval(this.interval$);
	}
}
