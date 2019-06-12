import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { RoleRoutingModule } from './role-routing.module';
import { RoleComponent } from './role.component';
import { AddRoleComponent } from './add-role/add-role.component';
import { EditRoleComponent } from './edit-role/edit-role.component';
import { STModule } from '@delon/abc';
import { NzCardModule, NzIconModule } from 'ng-zorro-antd';
import { SharedModule } from '@shared';

@NgModule({
	declarations: [ RoleComponent, AddRoleComponent, EditRoleComponent ],
	imports: [ CommonModule, RoleRoutingModule, STModule, NzCardModule, NzIconModule, SharedModule ]
})
export class RoleModule {}
