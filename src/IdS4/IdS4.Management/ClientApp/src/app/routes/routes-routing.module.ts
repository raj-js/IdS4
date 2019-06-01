import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { SimpleGuard } from '@delon/auth';
import { environment } from '@env/environment';
// layout
import { LayoutDefaultComponent } from '../layout/default/default.component';
import { LayoutFullScreenComponent } from '../layout/fullscreen/fullscreen.component';
import { LayoutPassportComponent } from '../layout/passport/passport.component';
// dashboard pages
import { DashboardComponent } from './dashboard/dashboard.component';
// passport pages
import { UserLoginComponent } from './passport/login/login.component';
import { UserRegisterComponent } from './passport/register/register.component';
import { UserRegisterResultComponent } from './passport/register-result/register-result.component';
import { SocialLoginComponent } from './passport/social-login/social-login.component';
import { UserLockComponent } from './passport/lock/lock.component';

// single pages
import { CallbackComponent } from './callback/callback.component';
import { AuthorizationGuard } from '@core/auth/authorization.guard';

const routes: Routes = [
	{
		path: '',
		component: LayoutDefaultComponent,
		canActivate: [ AuthorizationGuard ],
		canLoad: [ AuthorizationGuard ],
		children: [
			{ path: '', redirectTo: 'dashboard', pathMatch: 'full' },
			{ path: 'dashboard', component: DashboardComponent, data: { title: '仪表盘' } },
			{ path: 'exception', loadChildren: './exception/exception.module#ExceptionModule' },
			// resource
			{ path: 'resource', loadChildren: './resource/resource.module#ResourceModule' },
			// client
			{ path: 'client', loadChildren: './client/client.module#ClientModule' },
			// user
			{ path: 'user', loadChildren: './user/user.module#UserModule' },
			// role
			{ path: 'role', loadChildren: './role/role.module#RoleModule' }
		]
	},
	// passport
	{
		path: 'passport',
		component: LayoutPassportComponent,
		children: [
			{ path: 'login', component: UserLoginComponent, data: { title: '登录' } },
			{ path: 'register', component: UserRegisterComponent, data: { title: '注册' } },
			{ path: 'register-result', component: UserRegisterResultComponent, data: { title: '注册结果' } },
			{ path: 'lock', component: UserLockComponent, data: { title: '锁屏' } },
			{ path: 'social-login', component: SocialLoginComponent, data: { title: '登录' } }
		]
	},
	// 单页不包裹Layout
	{ path: 'callback', component: CallbackComponent },
	{ path: '**', redirectTo: 'exception/404' }
];

@NgModule({
	imports: [
		RouterModule.forRoot(routes, {
			useHash: environment.useHash,
			// NOTICE: If you use `reuse-tab` component and turn on keepingScroll you can set to `disabled`
			// Pls refer to https://ng-alain.com/components/reuse-tab
			scrollPositionRestoration: 'top'
		})
	],
	exports: [ RouterModule ]
})
export class RouteRoutingModule {}
