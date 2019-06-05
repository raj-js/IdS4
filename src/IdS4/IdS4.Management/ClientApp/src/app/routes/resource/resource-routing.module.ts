import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ApiResourceComponent } from './api-resource/api-resource.component';
import { IdentityResourceComponent } from './identity-resource/identity-resource.component';
import { AddIdentityResourceComponent } from './add-identity-resource/add-identity-resource.component';
import { EditIdentityResourceComponent } from './edit-identity-resource/edit-identity-resource.component';
import { AddApiResourceComponent } from './add-api-resource/add-api-resource.component';
import { EditApiResourceComponent } from './edit-api-resource/edit-api-resource.component';

const routes: Routes = [
	{ path: 'api', component: ApiResourceComponent },
	{ path: 'identity', component: IdentityResourceComponent },
	{ path: 'add-identity', component: AddIdentityResourceComponent, data: { title: '新建身份资源' } },
	{ path: 'edit-identity/:id', component: EditIdentityResourceComponent, data: { title: '编辑身份资源' } },
	{ path: 'add-api', component: AddApiResourceComponent, data: { title: '新建Api资源' } },
	{ path: 'edit-api/:id', component: 	EditApiResourceComponent, data: { title: '编辑Api资源' } }
];

@NgModule({
	imports: [ RouterModule.forChild(routes) ],
	exports: [ RouterModule ]
})
export class ResourceRoutingModule {}
