import { Component, OnInit, Renderer2, ElementRef } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';
import { filter } from 'rxjs/operators';
import { TitleService } from '@delon/theme';
import { VERSION as VERSION_ALAIN } from '@delon/theme';
import { VERSION as VERSION_ZORRO, NzModalService } from 'ng-zorro-antd';
import { ConfigurationService } from '@shared/services/configuration.service';
import { OidcSecurityService } from 'angular-auth-oidc-client';

@Component({
	selector: 'app-root',
	template: `
    <router-outlet></router-outlet>
  `
})
export class AppComponent implements OnInit {
	constructor(
		el: ElementRef,
		renderer: Renderer2,
		private router: Router,
		private titleSrv: TitleService,
		private modalSrv: NzModalService,
		private configurationService: ConfigurationService,
		private oidcSecurityService: OidcSecurityService
	) {
		if (this.oidcSecurityService.moduleSetup) {
			this.oidcSecurityService.authorizedCallbackWithCode(window.location.toString());
		} else {
			this.oidcSecurityService.onModuleSetup.subscribe(() => {
				this.oidcSecurityService.authorizedCallbackWithCode(window.location.toString());
			});
		}

		renderer.setAttribute(el.nativeElement, 'ng-alain-version', VERSION_ALAIN.full);
		renderer.setAttribute(el.nativeElement, 'ng-zorro-version', VERSION_ZORRO.full);
	}

	ngOnInit() {
		this.configurationService.load();

		this.router.events.pipe(filter((evt) => evt instanceof NavigationEnd)).subscribe(() => {
			this.titleSrv.setTitle();
			this.modalSrv.closeAll();
		});
	}
}
