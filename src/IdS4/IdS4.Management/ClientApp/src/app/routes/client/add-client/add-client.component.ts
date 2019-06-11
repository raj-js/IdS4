import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NzMessageService } from 'ng-zorro-antd';
import { ClientType } from '@shared/models/client-type.enum';
import { SFSchema } from '@delon/form';
import { ConfigurationService } from '@shared/services/configuration.service';
import { _HttpClient } from '@delon/theme';
import { IApiResult } from '@shared/models/api-result.model';
import { ApiResultCode } from '@shared/models/api-result-code.enum';

@Component({
	selector: 'app-add-client',
	templateUrl: './add-client.component.html',
	styles: []
})
export class AddClientComponent implements OnInit {
	url: string;
	saving = false;

	schema: SFSchema = {
		properties: {
			clientId: {
				type: 'string',
				title: '客户端标识',
				maxLength: 32,
				ui: {
					autofocus: true,
					grid: {
						span: 4
					}
				}
			},
			clientName: { type: 'string', title: '客户端名称', maxLength: 32 },
			type: {
				title: '初始化类型',
				type: 'string',
				maxLength: 32,
				enum: [
					{ label: 'Empty', value: ClientType.Empty },
					{ label: 'Hybrid', value: ClientType.Hybrid },
					{ label: 'SPA', value: ClientType.SPA },
					{ label: 'Native', value: ClientType.Native },
					{ label: 'Machine', value: ClientType.Machine },
					{ label: 'Device', value: ClientType.Device }
				],
				ui: {
					widget: 'select'
				},
				default: ClientType.Empty
			}
		},
		required: [ 'clientId', 'clientName' ]
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
		this.http.post(`${this.url}/api/client`, value).subscribe((resp) => {
			const result = resp as IApiResult;
			if (result.code === ApiResultCode.Success) {
				this.msgSrv.success('操作成功');
				setTimeout(() => {
					this.router.navigateByUrl(`/client/edit/${result.data.id}`);
				}, 500);
			} else {
				this.msgSrv.error(`code: ${result.code} \r\n errors: ${JSON.stringify(result.errors)}`);
			}
			this.cdr.detectChanges();
		});
	}
}
