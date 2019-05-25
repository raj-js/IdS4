import { Injectable } from '@angular/core';
import { UserManager, User } from 'oidc-client';
import { ReplaySubject } from 'rxjs';

@Injectable()
export class OidcService {
	private userManager: UserManager;
	private currentUser: User;

	userLoaded$ = new ReplaySubject<boolean>(1);

	jsClientSettings = {
		authority: 'https://localhost:5001',
		client_id: 'IdS4.Management.Spa',
		redirect_uri: 'https://localhost:5002/#/callback',
		response_type: 'code',
		scope: 'coreApi.full_access',
		post_logout_redirect_uri: 'https://localhost:5002/#/index',
		filterProtocolClaims: true
	};

	constructor() {
		this.userManager = new UserManager(this.jsClientSettings);
		this.userManager.clearStaleState();

		this.userManager.events.addUserLoaded((user) => {
			this.currentUser = user;
			console.log(user);
			this.userLoaded$.next(true);
		});

		this.userManager.events.addUserUnloaded((ev) => {
			this.currentUser = null;
			this.userLoaded$.next(true);
		});
	}

	public isAuthenticate(): boolean {
		return this.currentUser != null;
	}

	public getUser(): Promise<User> {
		return this.userManager.getUser();
	}

	public signIn(): Promise<void> {
		return this.userManager.signinRedirect();
	}

	public signOut(): Promise<void> {
		return this.userManager.signoutRedirect();
	}

	public renewToken(): Promise<User> {
		return this.userManager.signinSilent();
	}
}
