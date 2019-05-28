import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ApiResourceComponent } from './api-resource/api-resource.component';
import { IdentityResourceComponent } from './identity-resource/identity-resource.component';

const routes: Routes = [
	{ path: 'api', component: ApiResourceComponent },
	{ path: 'identity', component: IdentityResourceComponent }
];

@NgModule({
	imports: [ RouterModule.forChild(routes) ],
	exports: [ RouterModule ]
})
export class ResourceRoutingModule {}
