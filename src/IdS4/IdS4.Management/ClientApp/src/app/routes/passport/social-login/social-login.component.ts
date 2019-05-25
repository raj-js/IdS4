import { Component, OnInit } from '@angular/core';
import { SocialService } from '@delon/auth';
import { OidcService } from '@shared/services/oidc.service';

@Component({
	selector: 'app-social-login',
	templateUrl: './social-login.component.html',
	styles: [],
	providers: [ SocialService ]
})
export class SocialLoginComponent implements OnInit {
	constructor(private oidcService: OidcService) {}

	ngOnInit() {
		this.oidcService.signIn().then(() => {
			console.log('sign in...');
		});
	}
}
