import { Component, OnInit, ViewChild, ChangeDetectorRef } from '@angular/core';
import { Observable } from 'rxjs';
import { STData, STColumn, STComponent, STReq, STRes, STChange } from '@delon/abc';
import { Router } from '@angular/router';
import { ConfigurationService } from '@shared/services/configuration.service';
import { NzMessageService } from 'ng-zorro-antd';
import { _HttpClient } from '@delon/theme';
import { IApiResult } from '@shared/models/api-result.model';
import { ApiResultCode } from '@shared/models/api-result-code.enum';

@Component({
	selector: 'app-api-resource',
	templateUrl: './api-resource.component.html',
	styles: []
})
export class ApiResourceComponent implements OnInit {
	url: string;

	@ViewChild('st') st: STComponent;

	loading = false;
	query: any = {
		skip: 0,
		limit: 10
	};

	req: STReq = {
		type: 'skip',
		method: 'GET'
	};

	resp: STRes = {
		reName: {
			total: 'total',
			list: 'list'
		}
	};

	resources: Observable<STData[]>;
	checked: STData[];

	columns: STColumn[] = [
		{ title: '', type: 'checkbox', index: 'key' },
		{ title: 'ID', index: 'id' },
		{ title: '名称', index: 'name' },
		{ title: '显示名称', index: 'displayName' },
		{ title: '创建时间', index: 'created', type: 'date' },
		{ title: '修改时间', index: 'updated', type: 'date' },
		{ title: '最后访问时间', index: 'lastAccessed', type: 'date' },
		{ title: '是否启用', index: 'enabled', type: 'yn' },
		{
			title: '',
			buttons: [
				{
					text: '修改',
					icon: 'edit',
					type: 'link',
					iif: (row: any) => !row.nonEditable,
					click: (row: any) => this.router.navigateByUrl(`/resource/edit-api/${row.id}`)
				}
			]
		}
	];

	constructor(
		private configSrv: ConfigurationService,
		private router: Router,
		private msgSrv: NzMessageService,
		private http: _HttpClient,
		private cdr: ChangeDetectorRef
	) {}

	ngOnInit() {
		if (this.configSrv.isReady) {
			this.url = `${this.configSrv.serverSettings.coreApiUrl}/api/resource/api`;
		} else {
			this.configSrv.settingsLoaded$.subscribe(() => {
				this.url = `${this.configSrv.serverSettings.coreApiUrl}/api/resource/api`;
			});
		}
	}

	change(e: STChange) {
		if (e.checkbox) {
			this.checked = e.checkbox;
		}
	}

	add(): void {
		this.router.navigate([ '/resource/add-api' ]);
	}

	remove(): void {
		if (this.checked.length === 0) {
			this.msgSrv.warning('未选择任何身份资源');
			return;
		}

		this.loading = true;
		const ids = [];
		this.checked.forEach((v, i, d) => ids.push(v.id));
		this.http.delete(`${this.url}/${ids.join(',')}`).subscribe((resp) => {
			this.loading = false;
			const result = resp as IApiResult;
			if (result.code === ApiResultCode.Success) {
				this.msgSrv.success('操作成功');
				setTimeout(() => {
					this.st.reload();
				}, 500);
			} else {
				this.msgSrv.error(`code: ${result.code} \r\n errors: ${JSON.stringify(result.errors)}`);
			}
			this.cdr.detectChanges();
		});
	}

	//#region privates
	//#endregion
}
