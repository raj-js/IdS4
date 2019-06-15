import { Component, OnInit, ViewChild, ChangeDetectorRef } from '@angular/core';
import { SFComponent, SFSchema } from '@delon/form';
import { NzMessageService } from 'ng-zorro-antd';
import { ConfigurationService } from '@shared/services/configuration.service';
import { _HttpClient } from '@delon/theme';
import { IApiResult } from '@shared/models/api-result.model';
import { ApiResultCode } from '@shared/models/api-result-code.enum';
import { ActivatedRoute } from '@angular/router';

@Component({
	selector: 'app-edit-identity-resource',
	templateUrl: './edit-identity-resource.component.html',
	styles: []
})
export class EditIdentityResourceComponent implements OnInit {
	id: number;
	@ViewChild('basic') basic: SFComponent;

	url: string;
	loadingBasic = false;

	resource: any;

	basicSchema: SFSchema = {
		properties: {
			id: {
				type: 'number',
				title: 'ID',
				ui: {
					widget: 'text'
				}
			},
			name: {
				type: 'string',
				title: '名称',
				maxLength: 32,
				ui: {
					autofocus: true,
					grid: {
						span: 4
					}
				}
			},
			displayName: { type: 'string', title: '显示名称', maxLength: 32 },
			description: {
				type: 'string',
				title: '描述',
				maxLength: 256,
				ui: {
					widget: 'textarea',
					autosize: {
						minRows: 3,
						maxRows: 6
					}
				}
			},
			required: { type: 'boolean', title: '必选', default: false },
			emphasize: { type: 'boolean', title: '强调', default: false },
			showInDiscoveryDocument: { type: 'boolean', title: '显示在发现文档中', default: true },
			enabled: { type: 'boolean', title: '是否启用', default: true },
			userClaims: {
				title: '用户声明',
				type: 'array',
				items: {
					type: 'object',
					properties: {
						id: { title: 'ID', type: 'number', default: 0, ui: { widget: 'text' } },
						identityResourceId: { type: 'number', ui: { hidden: true } },
						type: { title: '类型', type: 'string', maxLength: 256 }
					},
					required: [ 'type' ]
				}
			},
			properties: {
				title: '资源属性',
				type: 'array',
				items: {
					type: 'object',
					properties: {
						id: { title: 'ID', type: 'number', default: 0, ui: { widget: 'text' } },
						identityResourceId: { type: 'number', ui: { hidden: true } },
						key: { title: '键', type: 'string', maxLength: 32 },
						value: { title: '值', type: 'string', maxLength: 256 }
					},
					required: [ 'key' ]
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
		this.id = Number.parseInt(this.route.snapshot.paramMap.get('id'), 10);

		if (this.id === Number.NaN) {
			this.msgSrv.error('身份资源ID无效！');
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
		this.loadingBasic = true;
		this.http.get(`${this.url}/api/resource/identity/${this.id}`).subscribe((resp) => {
			this.loadingBasic = false;
			const result = resp as IApiResult;
			if (result.code === ApiResultCode.Success) {
				this.resource = result.data;
			} else {
				this.msgSrv.error(`code: ${result.code} \r\n errors: ${JSON.stringify(result.errors)}`);
			}
			this.cdr.detectChanges();
		});
	}

	submit(value: any): void {
		this.loadingBasic = true;
		this.http.put(`${this.url}/api/resource/identity`, value).subscribe((resp) => {
			this.loadingBasic = false;
			const result = resp as IApiResult;
			if (result.code === ApiResultCode.Success) {
				this.resource = result.data;
				this.msgSrv.success('操作成功');
			} else {
				this.msgSrv.error(`code: ${result.code} \r\n errors: ${JSON.stringify(result.errors)}`);
			}
			this.cdr.detectChanges();
		});
	}
}
