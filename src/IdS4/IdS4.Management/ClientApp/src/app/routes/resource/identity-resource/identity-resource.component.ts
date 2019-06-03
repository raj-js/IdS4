import { Component, OnInit, TemplateRef, ChangeDetectorRef, ChangeDetectionStrategy } from '@angular/core';
import { STColumn, STReq, STRes, STData } from '@delon/abc';
import { _HttpClient } from '@delon/theme';
import { NzMessageService, NzModalService } from 'ng-zorro-antd';
import { map, tap } from 'rxjs/operators';
import { ConfigurationService } from '@shared/services/configuration.service';
import { Observable } from 'rxjs';
import { Router } from '@angular/router';

@Component({
	selector: 'app-identity-resource',
	templateUrl: './identity-resource.component.html',
	styles: [],
	changeDetection: ChangeDetectionStrategy.OnPush
})
export class IdentityResourceComponent implements OnInit {
	url: string;
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

	columns: STColumn[] = [
		{ title: '', type: 'checkbox', index: 'key' },
		{ title: 'ID', index: 'id' },
		{ title: '名称', index: 'name' },
		{ title: '显示名称', index: 'displayName' },
		{ title: '必选', index: 'required', type: 'yn' },
		{ title: '强调', index: 'emphasize', type: 'yn' },
		{ title: '显示在发现文档中', index: 'showInDiscoveryDocument', type: 'yn' },
		{ title: '创建时间', index: 'created', type: 'date' },
		{ title: '修改时间', index: 'updated', type: 'date' },
		{ title: '是否启用', index: 'enabled', type: 'yn' },
		{
			title: '',
			buttons: [
				{
					text: '修改',
					icon: 'edit',
					type: 'link',
					iif: (row: any) => !row.nonEditable,
					click: (row: any) => this.router.navigate([ '/resource/edit-identity', { id: row.id } ])
				}
			]
		}
	];

	constructor(private configSrv: ConfigurationService, private router: Router) {}

	ngOnInit() {
		if (this.configSrv.isReady) {
			this.url = `${this.configSrv.serverSettings.coreApiUrl}/api/resource/identity`;
		} else {
			this.configSrv.settingsLoaded$.subscribe(() => {
				this.url = `${this.configSrv.serverSettings.coreApiUrl}/api/resource/identity`;
			});
		}
	}

	add() {
		this.router.navigate([ '/resource/add-identity' ]);
	}

	//#region privates
	//#endregion
}
