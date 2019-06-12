import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { SFSchema } from '@delon/form';
import { NzMessageService } from 'ng-zorro-antd';
import { ConfigurationService } from '@shared/services/configuration.service';
import { _HttpClient } from '@delon/theme';
import { ActivatedRoute } from '@angular/router';
import { IApiResult } from '@shared/models/api-result.model';
import { ApiResultCode } from '@shared/models/api-result-code.enum';

@Component({
	selector: 'app-edit-role',
	templateUrl: './edit-role.component.html',
	styles: []
})
export class EditRoleComponent implements OnInit {
	id: string;

	url: string;
	loading = false;

	form: any;

	schema: SFSchema = {
		properties: {
			id: { type: 'string', ui: { hidden: true } },
			name: { type: 'string', title: '用户名', readOnly: true },
			roleClaims: {
				title: '用户声明',
				type: 'array',
				items: {
					type: 'object',
					properties: {
						id: { title: 'ID', type: 'number', default: 0, ui: { widget: 'text' } },
						userId: { type: 'number', ui: { hidden: true } },
						claimType: { title: '类型', type: 'string', maxLength: 250 },
						claimValue: { title: '值', type: 'string', maxLength: 250 }
					},
					required: [ 'claimType', 'claimValue' ]
				}
			}
		},
		required: [ 'name' ]
	};

	constructor(
		private msgSrv: NzMessageService,
		private configSrv: ConfigurationService,
		private http: _HttpClient,
		private cdr: ChangeDetectorRef,
		private route: ActivatedRoute
	) {}

	ngOnInit() {
		this.id = this.route.snapshot.paramMap.get('id');

		if (this.id === undefined || this.id === null || this.id === '') {
			this.msgSrv.error('角色ID无效！');
			return;
		}

		if (this.configSrv.isReady) {
			this.url = this.configSrv.serverSettings.coreApiUrl;
			this.load();
		} else {
			this.configSrv.settingsLoaded$.subscribe(() => {
				this.url = this.configSrv.serverSettings.coreApiUrl;
				this.load();
			});
		}
	}

	load(): void {
		this.loading = true;
		this.http.get(`${this.url}/api/role/${this.id}`).subscribe((resp) => {
			this.loading = false;
			const result = resp as IApiResult;
			if (result.code === ApiResultCode.Success) {
				this.form = result.data;
			} else {
				this.msgSrv.error(`code: ${result.code} \r\n errors: ${JSON.stringify(result.errors)}`);
			}
			this.cdr.detectChanges();
		});
	}

	submit(value: any): void {
		this.loading = true;
		this.http.put(`${this.url}/api/role`, value).subscribe((resp) => {
			this.loading = false;
			const result = resp as IApiResult;
			if (result.code === ApiResultCode.Success) {
				this.form = result.data;
				this.msgSrv.success('操作成功');
			} else {
				this.msgSrv.error(`code: ${result.code} \r\n errors: ${JSON.stringify(result.errors)}`);
			}
			this.cdr.detectChanges();
		});
	}
}
