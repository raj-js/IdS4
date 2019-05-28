import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ResourceRoutingModule } from './resource-routing.module';
import { IdentityResourceComponent } from './identity-resource/identity-resource.component';
import { ApiResourceComponent } from './api-resource/api-resource.component';

@NgModule({
	declarations: [ IdentityResourceComponent, ApiResourceComponent ],
	imports: [ CommonModule, ResourceRoutingModule ]
})
export class ResourceModule {}
