import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ClientRoutingModule } from './client-routing.module';
import { ClientComponent } from './client.component';
import { STModule } from '@delon/abc';
import { NzCardModule, NzIconModule } from 'ng-zorro-antd';
import { SharedModule } from '@shared';

@NgModule({
	declarations: [ ClientComponent ],
	imports: [ CommonModule, ClientRoutingModule, STModule, NzCardModule, NzIconModule, SharedModule ]
})
export class ClientModule {}
