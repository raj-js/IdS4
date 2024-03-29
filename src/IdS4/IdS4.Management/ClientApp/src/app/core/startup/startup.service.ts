import { Injectable, Injector, Inject } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { zip } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { MenuService, SettingsService, TitleService, ALAIN_I18N_TOKEN } from '@delon/theme';
import { DA_SERVICE_TOKEN, ITokenService, JWTTokenModel } from '@delon/auth';
import { ACLService } from '@delon/acl';
import { TranslateService } from '@ngx-translate/core';
import { I18NService } from '../i18n/i18n.service';

import { NzIconService } from 'ng-zorro-antd/icon';
import { ICONS_AUTO } from '../../../style-icons-auto';
import { ICONS } from '../../../style-icons';

import { OidcSecurityService } from 'angular-auth-oidc-client';

/**
 * 用于应用启动时
 * 一般用来获取应用所需要的基础数据等
 */
@Injectable()
export class StartupService {
	private oidcSecurityService: OidcSecurityService;

	constructor(
		iconSrv: NzIconService,
		private menuService: MenuService,
		private translate: TranslateService,
		@Inject(ALAIN_I18N_TOKEN) private i18n: I18NService,
		private settingService: SettingsService,
		private aclService: ACLService,
		private titleService: TitleService,
		private httpClient: HttpClient,
		private injector: Injector
	) {
		iconSrv.addIcon(...ICONS_AUTO, ...ICONS);
	}

	private viaHttp(resolve: any, reject: any) {
		zip(
			this.httpClient.get(`assets/tmp/i18n/${this.i18n.defaultLang}.json`),
			this.httpClient.get(`assets/tmp/app-data.json?v=${new Date().getTime()}`)
		)
			.pipe(
				// 接收其他拦截器后产生的异常消息
				catchError(([ langData, appData ]) => {
					resolve(null);
					return [ langData, appData ];
				})
			)
			.subscribe(
				([ langData, appData ]) => {
					// setting language data
					this.translate.setTranslation(this.i18n.defaultLang, langData);
					this.translate.setDefaultLang(this.i18n.defaultLang);

					// application data
					const res: any = appData;
					// 应用信息：包括站点名、描述、年份
					this.settingService.setApp(res.app);
					// 用户信息：包括姓名、头像、邮箱地址
					// this.settingService.setUser(res.user);
					this.setUser();
					// ACL：设置权限为全量
					this.aclService.setFull(true);
					// 初始化菜单
					this.menuService.add(res.menu);
					// 设置页面标题的后缀
					this.titleService.suffix = res.app.name;
				},
				() => {},
				() => {
					resolve(null);
				}
			);
	}

	private setUser() {
		this.oidcSecurityService = this.injector.get(OidcSecurityService);
		if (this.oidcSecurityService === null) return;

		const token = this.oidcSecurityService.getToken();
		if (token === null || token === '') return;

		this.oidcSecurityService.getUserData().subscribe((d) => {
			this.settingService.setUser({
				name: d.name,
				email: d.email,
				avatar: d.avatar
			});
		});
	}

	load(): Promise<any> {
		// only works with promises
		// https://github.com/angular/angular/issues/15088
		return new Promise((resolve, reject) => {
			// http
			this.viaHttp(resolve, reject);
			// mock：请勿在生产环境中这么使用，viaMock 单纯只是为了模拟一些数据使脚手架一开始能正常运行
			// this.viaMockI18n(resolve, reject);
		});
	}
}
