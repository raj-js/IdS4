import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { UserComponent } from './user.component';
import { AddUserComponent } from './add-user/add-user.component';
import { EditUserComponent } from './edit-user/edit-user.component';

const routes: Routes = [
	{ path: '', component: UserComponent },
	{ path: 'add', component: AddUserComponent, data: { title: '新建用户' } },
	{ path: 'edit/:id', component: EditUserComponent, data: { title: '编辑用户' } }
];

@NgModule({
	imports: [ RouterModule.forChild(routes) ],
	exports: [ RouterModule ]
})
export class UserRoutingModule {}
