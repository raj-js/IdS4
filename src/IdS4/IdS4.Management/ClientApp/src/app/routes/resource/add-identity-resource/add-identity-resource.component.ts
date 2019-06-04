import { Component, OnInit, ChangeDetectionStrategy, ElementRef, ViewChild, ChangeDetectorRef } from '@angular/core';
import { SFSchema, SFUISchema, SFComponent } from '@delon/form';
import { NzMessageService } from 'ng-zorro-antd';
import { ConfigurationService } from '@shared/services/configuration.service';
import { _HttpClient } from '@delon/theme';
import { zip } from 'rxjs/operators';
import { IApiResult } from '@shared/models/api-result.model';
import { ApiResultCode } from '@shared/models/api-result-code.enum';
import { Router } from '@angular/router';

@Component({
	selector: 'app-add-identity-resource',
	templateUrl: './add-identity-resource.component.html',
	styles: [],
	changeDetection: ChangeDetectionStrategy.OnPush
})
export class AddIdentityResourceComponent implements OnInit {
	url: string;
	saving = false;
	basicSaved = false;

	basicSchema: SFSchema = {
		properties: {
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
			enabled: { type: 'boolean', title: '是否启用', default: true }
		},
		required: [ 'name', 'displayName' ]
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

	basicSubmit(value: any): void {
		this.saving = true;
		this.http.post(`${this.url}/api/resource/identity/add`, value).subscribe((resp) => {
			const result = resp as IApiResult;
			if (result.code === ApiResultCode.Success) {
				this.msgSrv.success('操作成功');
				setTimeout(() => {
					this.router.navigateByUrl(`/resource/edit-identity/${result.data.id}`);
				}, 500);
			} else {
				this.msgSrv.error(`code: ${result.code} \r\n errors: ${JSON.stringify(result.errors)}`);
			}
			this.cdr.detectChanges();
		});
	}
}
