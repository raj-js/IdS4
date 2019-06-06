import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ClientComponent } from './client.component';
import { AddClientComponent } from './add-client/add-client.component';
import { EditClientComponent } from './edit-client/edit-client.component';

const routes: Routes = [
	{ path: '', component: ClientComponent },
	{ path: 'add', component: AddClientComponent },
	{ path: 'edit/:id', component: EditClientComponent }
];

@NgModule({
	imports: [ RouterModule.forChild(routes) ],
	exports: [ RouterModule ]
})
export class ClientRoutingModule {}
