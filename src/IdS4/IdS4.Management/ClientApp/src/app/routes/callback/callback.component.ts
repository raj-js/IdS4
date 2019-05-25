import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { SocialService } from '@delon/auth';
import { SettingsService } from '@delon/theme';
import { OidcService } from '@shared/services/oidc.service';

@Component({
	selector: 'app-callback',
	template: `<Button type="primary" (click)="logout()">logout</Button>`,
	providers: [ SocialService ]
})
export class CallbackComponent implements OnInit {
	constructor(
		private socialService: SocialService,
		private settingsSrv: SettingsService,
		private route: ActivatedRoute,
		private oidcService: OidcService
	) {}

	ngOnInit(): void {
		// this.mockModel();
		this.oidcService
			.getUser()
			.then((user) => {
				console.log(user);
			})
			.catch((error) => {
				console.log(`error: ${error}`);
			});
	}

	logout() {
		this.oidcService.signOut().then(() => {
			console.log('logout...');
		});
	}

	private mockModel() {
		// const info = {
		// 	token: '123456789',
		// 	name: 'cipchk',
		// 	email: `${this.type}@${this.type}.com`,
		// 	id: 10000,
		// 	time: +new Date()
		// };
		// this.settingsSrv.setUser({
		// 	...this.settingsSrv.user,
		// 	...info
		// });
		// this.socialService.callback(info);
	}
}
