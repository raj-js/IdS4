import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ResourceRoutingModule } from './resource-routing.module';
import { IdentityResourceComponent } from './identity-resource/identity-resource.component';
import { ApiResourceComponent } from './api-resource/api-resource.component';
import { STModule } from '@delon/abc';
import { NzCardModule } from 'ng-zorro-antd';
import { SharedModule } from '@shared';

@NgModule({
	declarations: [ IdentityResourceComponent, ApiResourceComponent ],
	imports: [ CommonModule, ResourceRoutingModule, STModule, NzCardModule, SharedModule ]
})
export class ResourceModule {}
