import { Component, OnInit, ViewChild, ChangeDetectorRef } from '@angular/core';
import { SFComponent, SFSchema } from '@delon/form';
import { NzMessageService } from 'ng-zorro-antd';
import { ConfigurationService } from '@shared/services/configuration.service';
import { _HttpClient } from '@delon/theme';
import { ActivatedRoute } from '@angular/router';
import { IApiResult } from '@shared/models/api-result.model';
import { ApiResultCode } from '@shared/models/api-result-code.enum';

@Component({
	selector: 'app-edit-user',
	templateUrl: './edit-user.component.html',
	styles: []
})
export class EditUserComponent implements OnInit {
	id: string;

	url: string;
	loading = false;

	form: any;

	schema: SFSchema = {
		properties: {
			id: { type: 'string', ui: { hidden: true } },
			userName: { type: 'string', title: '用户名', readOnly: true },
			email: { type: 'string', title: '邮箱地址', readOnly: true },
			emailConfirmed: { type: 'boolean', title: '邮箱是否确认' },
			twoFactorEnabled: { type: 'boolean', title: '启用双重验证' },
			lockoutEnabled: { type: 'boolean', title: '启用锁定' },
			claims: {
				title: '用户声明',
				type: 'array',
				items: {
					type: 'object',
					properties: {
						id: { type: 'number', ui: { hidden: true } },
						userId: { type: 'number', ui: { hidden: true } },
						claimType: { title: '键', type: 'string', maxLength: 2000 },
						claimValue: { title: '值', type: 'string', maxLength: 2000 }
					},
					required: [ 'claimType' ]
				}
			}
		},
		required: [ 'name', 'displayName' ]
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
			this.msgSrv.error('用户ID无效！');
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
		this.http.get(`${this.url}/api/user/${this.id}`).subscribe((resp) => {
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
		this.http.put(`${this.url}/api/user`, value).subscribe((resp) => {
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
