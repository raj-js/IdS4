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

	public isAuthenticate(): boolean {
		return this.currentUser != null;
	}

	public getUser(): User {
		return this.currentUser;
	}

	public signIn(): Promise<void> {
		return this.userManager.signinRedirect();
	}

	public signOut(): Promise<void> {
		return this.userManager.signoutRedirect();
	}

	public refresh(): Promise<User> {
		return this.userManager.signinSilent();
	}
}
