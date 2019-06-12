import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { SFSchema } from '@delon/form';
import { NzMessageService } from 'ng-zorro-antd';
import { ConfigurationService } from '@shared/services/configuration.service';
import { _HttpClient } from '@delon/theme';
import { Router } from '@angular/router';
import { IApiResult } from '@shared/models/api-result.model';
import { ApiResultCode } from '@shared/models/api-result-code.enum';

@Component({
	selector: 'app-add-role',
	templateUrl: './add-role.component.html',
	styles: []
})
export class AddRoleComponent implements OnInit {
	url: string;
	saving = false;

	schema: SFSchema = {
		properties: {
			name: { type: 'string', title: '角色', maxLength: 256 }
		},
		required: [ 'name' ]
	};

	constructor(
		private msgSrv: NzMessageService,
		private configSrv: ConfigurationService,
		private http: _HttpClient,
		private cdr: ChangeDetectorRef,
		private router: Router
	) {}

	ngOnInit() {
		if (this.configSrv.isReady) {
			this.url = this.configSrv.serverSettings.coreApiUrl;
		} else {
			this.configSrv.settingsLoaded$.subscribe(() => {
				this.url = this.configSrv.serverSettings.coreApiUrl;
			});
		}
	}

	submit(value: any): void {
		this.saving = true;
		this.http.post(`${this.url}/api/role`, value).subscribe((resp) => {
			const result = resp as IApiResult;
			if (result.code === ApiResultCode.Success) {
				this.msgSrv.success('操作成功');
				setTimeout(() => {
					this.router.navigateByUrl(`/role/edit/${result.data.id}`);
				}, 500);
			} else {
				this.msgSrv.error(`code: ${result.code} \r\n errors: ${JSON.stringify(result.errors)}`);
			}
			this.cdr.detectChanges();
		});
	}
}
