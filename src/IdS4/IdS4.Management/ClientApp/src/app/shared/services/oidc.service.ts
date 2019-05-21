import { Injectable } from '@angular/core';
import { UserManager, User } from 'oidc-client';
import { ReplaySubject } from 'rxjs';
import { ConfigurationService } from './configuration.service';

@Injectable()
export class OidcService {
	private userManager: UserManager;
	private currentUser: User;

	userLoaded$ = new ReplaySubject<boolean>(1);

	constructor(private configurationService: ConfigurationService) {
		this.configurationService.settingsLoaded$.subscribe((_) => {
			this.userManager = new UserManager(this.configurationService.serverSettings.oidcSpaClientSettings);
			this.userManager.clearStaleState();

			this.userManager.events.addUserLoaded((user) => {
				this.currentUser = user;
				this.userLoaded$.next(true);
			});

			this.userManager.events.addUserUnloaded((ev) => {
				this.currentUser = null;
				this.userLoaded$.next(true);
			});
		});
	}
}
