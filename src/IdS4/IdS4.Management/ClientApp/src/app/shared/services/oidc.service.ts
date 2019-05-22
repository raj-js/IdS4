import { Injectable } from '@angular/core';
import { UserManager, User } from 'oidc-client';
import { ReplaySubject } from 'rxjs';

@Injectable()
export class OidcService {
	private userManager: UserManager;
	private currentUser: User;

  userLoaded$ = new ReplaySubject<boolean>(1);

  jsClientSettings = {
    authority: "https://localhost:5001",
    client_id: "IdS4.Management.Spa",
    redirect_uri: "http://localhost:5002/",
    response_type: "code id_token",
    scope: "openid profile email roles coreApi",
    post_logout_redirect_uri: "http://localhost:5002/",
  };

	constructor() {
      this.userManager = new UserManager(this.jsClientSettings);
      this.userManager.clearStaleState();

      this.userManager.events.addUserLoaded((user) => {
        this.currentUser = user;
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
