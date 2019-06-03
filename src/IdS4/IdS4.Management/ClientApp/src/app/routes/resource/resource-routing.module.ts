import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ApiResourceComponent } from './api-resource/api-resource.component';
import { IdentityResourceComponent } from './identity-resource/identity-resource.component';
import { AddIdentityResourceComponent } from './add-identity-resource/add-identity-resource.component';
import { EditIdentityResourceComponent } from './edit-identity-resource/edit-identity-resource.component';

const routes: Routes = [
	{ path: 'api', component: ApiResourceComponent },
	{ path: 'identity', component: IdentityResourceComponent },
	{ path: 'add-identity', component: AddIdentityResourceComponent, data: { title: '新建身份资源' } },
	{ path: 'edit-identity/:id', component: EditIdentityResourceComponent, data: { title: '编辑身份资源' } }
];

@NgModule({
	imports: [ RouterModule.forChild(routes) ],
	exports: [ RouterModule ]
})
export class ResourceRoutingModule {}
