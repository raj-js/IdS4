import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { RoleComponent } from './role.component';
import { AddRoleComponent } from './add-role/add-role.component';
import { EditRoleComponent } from './edit-role/edit-role.component';

const routes: Routes = [
	{ path: '', component: RoleComponent },
	{ path: 'add', component: AddRoleComponent, data: { title: '新建角色' } },
	{ path: 'edit/:id', component: EditRoleComponent, data: { title: '编辑角色' } }
];

@NgModule({
	imports: [ RouterModule.forChild(routes) ],
	exports: [ RouterModule ]
})
export class RoleRoutingModule {}
