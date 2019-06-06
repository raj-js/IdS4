import { Injectable } from '@angular/core';
import {
	CanActivate,
	CanLoad,
	Route,
	UrlSegment,
	ActivatedRouteSnapshot,
	RouterStateSnapshot,
	UrlTree,
	Router
} from '@angular/router';
import { Observable } from 'rxjs';
import { OidcSecurityService } from 'angular-auth-oidc-client';
import { map } from 'rxjs/operators';

@Injectable({
	providedIn: 'root'
})
export class AuthorizationGuard implements CanActivate, CanLoad {
	constructor(private router: Router, private oidcSecurityService: OidcSecurityService) {}

	canActivate(
		next: ActivatedRouteSnapshot,
		state: RouterStateSnapshot
	): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
		return this.checkUserIsAuthencated();
	}
	canLoad(route: Route, segments: UrlSegment[]): Observable<boolean> | Promise<boolean> | boolean {
		return this.checkUserIsAuthencated();
	}

	private checkUserIsAuthencated(): Observable<boolean> {
		return this.oidcSecurityService.getIsAuthorized().pipe(
			map((isAuth: boolean) => {
				if (!isAuth) {
					this.router.navigate([ '/passport/login' ]);
					return false;
				}
				return true;
			})
		);
	}
}
