import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ResourceRoutingModule } from './resource-routing.module';
import { IdentityResourceComponent } from './identity-resource/identity-resource.component';
import { ApiResourceComponent } from './api-resource/api-resource.component';
import { STModule } from '@delon/abc';
import { NzCardModule, NzIconModule } from 'ng-zorro-antd';
import { SharedModule } from '@shared';
import { AddIdentityResourceComponent } from './add-identity-resource/add-identity-resource.component';
import { EditIdentityResourceComponent } from './edit-identity-resource/edit-identity-resource.component';
import { AddApiResourceComponent } from './add-api-resource/add-api-resource.component';
import { EditApiResourceComponent } from './edit-api-resource/edit-api-resource.component';

@NgModule({
	declarations: [
		IdentityResourceComponent,
		ApiResourceComponent,
		AddIdentityResourceComponent,
		EditIdentityResourceComponent,
		AddApiResourceComponent,
		EditApiResourceComponent
	],
	imports: [ CommonModule, ResourceRoutingModule, STModule, NzCardModule, NzIconModule, SharedModule ]
})
export class ResourceModule {}
