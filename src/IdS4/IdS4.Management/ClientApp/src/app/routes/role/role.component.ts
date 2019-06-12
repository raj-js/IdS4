import { Component, OnInit, ViewChild, ChangeDetectorRef } from '@angular/core';
import { ClientType } from '@shared/models/client-type.enum';
import { STComponent, STReq, STRes, STData, STColumn, STChange } from '@delon/abc';
import { Observable } from 'rxjs';
import { ConfigurationService } from '@shared/services/configuration.service';
import { Router } from '@angular/router';
import { NzMessageService } from 'ng-zorro-antd';
import { _HttpClient } from '@delon/theme';
import { IApiResult } from '@shared/models/api-result.model';
import { ApiResultCode } from '@shared/models/api-result-code.enum';

@Component({
	selector: 'app-role',
	templateUrl: './role.component.html',
	styles: []
})
export class RoleComponent implements OnInit {
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
		{ title: 'ID', index: 'id', type: 'no' },
		{ title: '角色', index: 'name' },
		{
			title: '',
			buttons: [
				{
					text: '修改',
					icon: 'edit',
					type: 'link',
					click: (row: any) => this.router.navigateByUrl(`/role/edit/${row.id}`)
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
			this.url = `${this.configSrv.serverSettings.coreApiUrl}/api/role`;
		} else {
			this.configSrv.settingsLoaded$.subscribe(() => {
				this.url = `${this.configSrv.serverSettings.coreApiUrl}/api/role`;
			});
		}
	}

	change(e: STChange) {
		if (e.checkbox) {
			this.checked = e.checkbox;
		}
	}

	add(type: ClientType): void {
		this.router.navigateByUrl(`/role/add`);
	}

	remove(): void {
		if (this.checked.length === 0) {
			this.msgSrv.warning('未选择任何角色');
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
