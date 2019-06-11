import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ClientComponent } from './client.component';
import { AddClientComponent } from './add-client/add-client.component';
import { EditClientComponent } from './edit-client/edit-client.component';

const routes: Routes = [
	{ path: '', component: ClientComponent },
	{ path: 'add', component: AddClientComponent, data: { title: '新建客户端' } },
	{ path: 'edit/:id', component: EditClientComponent, data: { title: '编辑客户端' } }
];

@NgModule({
	imports: [ RouterModule.forChild(routes) ],
	exports: [ RouterModule ]
})
export class ClientRoutingModule {}
