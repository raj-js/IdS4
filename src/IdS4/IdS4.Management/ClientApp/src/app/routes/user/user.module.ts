import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { UserRoutingModule } from './user-routing.module';
import { UserComponent } from './user.component';
import { STModule } from '@delon/abc';
import { NzCardModule, NzIconModule } from 'ng-zorro-antd';
import { SharedModule } from '@shared';
import { AddUserComponent } from './add-user/add-user.component';
import { EditUserComponent } from './edit-user/edit-user.component';

@NgModule({
	declarations: [ UserComponent, AddUserComponent, EditUserComponent ],
	imports: [ CommonModule, UserRoutingModule, STModule, NzCardModule, NzIconModule, SharedModule ]
})
export class UserModule {}
