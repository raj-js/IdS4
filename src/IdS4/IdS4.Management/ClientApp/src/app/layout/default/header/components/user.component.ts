import { Component, Inject, ChangeDetectionStrategy } from '@angular/core';
import { Router } from '@angular/router';
import { SettingsService } from '@delon/theme';
import { DA_SERVICE_TOKEN, ITokenService } from '@delon/auth';
import { OidcSecurityService } from 'angular-auth-oidc-client';

@Component({
	selector: 'header-user',
	template: `
    <nz-dropdown nzPlacement="bottomRight">
      <div class="alain-default__nav-item d-flex align-items-center px-sm" nz-dropdown>
        <nz-avatar [nzSrc]="settings.user.avatar" nzSize="small" class="mr-sm"></nz-avatar>
        {{ settings.user.name }}
      </div>
      <div nz-menu class="width-sm">
        <div nz-menu-item (click)="logout()">
          <i nz-icon nzType="logout" class="mr-sm"></i>
          {{ 'menu.account.logout' | translate }}
        </div>
      </div>
    </nz-dropdown>
  `,
	changeDetection: ChangeDetectionStrategy.OnPush
})
export class HeaderUserComponent {
	constructor(
		public settings: SettingsService,
		@Inject(DA_SERVICE_TOKEN) private tokenService: ITokenService,
		private oidcSecurityService: OidcSecurityService
	) {}

	logout() {
		this.tokenService.clear();
		this.oidcSecurityService.logoff((url) => {
			console.log(url, window.location.origin);
			window.location.href = `${url}?returnUrl=${window.location.origin}`;
		});
	}
}
