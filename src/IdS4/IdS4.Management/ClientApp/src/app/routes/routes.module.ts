import { NgModule } from '@angular/core';

import { SharedModule } from '@shared';
import { RouteRoutingModule } from './routes-routing.module';
// dashboard pages
import { DashboardComponent } from './dashboard/dashboard.component';
// passport pages
import { UserLoginComponent } from './passport/login/login.component';
import { UserRegisterComponent } from './passport/register/register.component';
import { UserRegisterResultComponent } from './passport/register-result/register-result.component';
import { UserLockComponent } from './passport/lock/lock.component';
import { SocialLoginComponent } from './passport/social-login/social-login.component';
// single pages
import { CallbackComponent } from './callback/callback.component';

const COMPONENTS = [
	DashboardComponent,
	// passport pages
	UserLoginComponent,
	UserRegisterComponent,
	UserRegisterResultComponent,
	SocialLoginComponent,
	UserLockComponent,
	// single pages
	CallbackComponent
];
const COMPONENTS_NOROUNT = [];

@NgModule({
	imports: [ SharedModule.forRoot(), RouteRoutingModule ],
	declarations: [ ...COMPONENTS, ...COMPONENTS_NOROUNT ],
	entryComponents: COMPONENTS_NOROUNT
})
export class RoutesModule {}
