import { Component, OnInit, ViewChild, ChangeDetectorRef } from '@angular/core';
import { SFComponent, SFSchema } from '@delon/form';
import { NzMessageService } from 'ng-zorro-antd';
import { ConfigurationService } from '@shared/services/configuration.service';
import { _HttpClient } from '@delon/theme';
import { ActivatedRoute } from '@angular/router';
import { IApiResult } from '@shared/models/api-result.model';
import { ApiResultCode } from '@shared/models/api-result-code.enum';

@Component({
	selector: 'app-edit-api-resource',
	templateUrl: './edit-api-resource.component.html',
	styles: []
})
export class EditApiResourceComponent implements OnInit {
	id: number;
	@ViewChild('basic') basic: SFComponent;

	url: string;
	loadingBasic = false;
	loadingClaims = false;
	loadingProperties = false;

	resource: any;
	claims: any;
	properties: any;

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
			enabled: { type: 'boolean', title: '是否启用', default: true }
		},
		required: [ 'name', 'displayName' ]
	};

	claimSchema: SFSchema = {
		properties: {
			claims: {
				title: '资源声明',
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
			}
		},
		ui: { spanLabel: 2, grid: { arraySpan: 12 } }
	};

	propertySchema: SFSchema = {
		properties: {
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
		ui: { spanLabel: 2, grid: { arraySpan: 12 } }
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
			this.msgSrv.error('Api资源ID无效！');
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
		this.http.get(`${this.url}/api/resource/api/${this.id}`).subscribe((resp) => {
			this.loadingBasic = false;
			const result = resp as IApiResult;
			if (result.code === ApiResultCode.Success) {
				this.resource = result.data;
				this.claims = { claims: result.data.userClaims };
				this.properties = { properties: result.data.properties };
			} else {
				this.msgSrv.error(`code: ${result.code} \r\n errors: ${JSON.stringify(result.errors)}`);
			}
			this.cdr.detectChanges();
		});
	}

	submit(value: any): void {
		this.loadingBasic = true;
		this.http.put(`${this.url}/api/resource/api`, value).subscribe((resp) => {
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

	submitClaims(value: any): void {
		this.loadingClaims = true;

		const arrs = value.claims as any[];
		arrs.forEach((v, i, d) => (v.apiResourceId = this.id));

		this.http.put(`${this.url}/api/resource/api/claims/${this.id}`, arrs).subscribe((resp) => {
			this.loadingClaims = false;
			const result = resp as IApiResult;
			if (result.code === ApiResultCode.Success) {
				this.claims = { claims: result.data };
				this.msgSrv.success('操作成功');
			} else {
				this.msgSrv.error(`code: ${result.code} \r\n errors: ${JSON.stringify(result.errors)}`);
			}
			this.cdr.detectChanges();
		});
	}

	submitProperties(value: any): void {
		this.loadingProperties = true;
		const arrs = value.properties as any[];
		arrs.forEach((v, i, d) => (v.apiResourceId = this.id));

		this.http.put(`${this.url}/api/resource/api/properties/${this.id}`, arrs).subscribe((resp) => {
			this.loadingProperties = false;
			const result = resp as IApiResult;
			if (result.code === ApiResultCode.Success) {
				this.properties = { properties: result.data };
				this.msgSrv.success('操作成功');
			} else {
				this.msgSrv.error(`code: ${result.code} \r\n errors: ${JSON.stringify(result.errors)}`);
			}
			this.cdr.detectChanges();
		});
	}
}
