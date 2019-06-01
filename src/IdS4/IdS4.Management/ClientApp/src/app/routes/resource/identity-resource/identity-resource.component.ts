import { Component, OnInit, TemplateRef, ChangeDetectorRef, ChangeDetectionStrategy } from '@angular/core';
import { STColumn } from '@delon/abc';
import { _HttpClient } from '@delon/theme';
import { NzMessageService, NzModalService } from 'ng-zorro-antd';
import { map, tap } from 'rxjs/operators';

@Component({
	selector: 'app-identity-resource',
	templateUrl: './identity-resource.component.html',
	styles: [],
	changeDetection: ChangeDetectionStrategy.OnPush
})
export class IdentityResourceComponent implements OnInit {
	loading = false;
	query: any = {
		pi: 1,
		ps: 10,
		sorter: '',
		status: null,
		statusList: []
	};

	resources: any[];

	columns: STColumn[] = [
		{ title: '', type: 'checkbox', index: 'key' },
		{ title: 'ID', index: 'id' },
		{ title: '名称', index: 'name' },
		{ title: '显示名称', index: 'displayName' },
		{ title: '必选', index: 'required' },
		{ title: '强调', index: 'emphasize' },
		{ title: '显示在发现文档', index: 'showInDiscoveryDocument' },
		{ title: '创建时间', index: 'created' },
		{ title: '修改时间', index: 'updated' },
		{ title: '不可修改', index: 'nonEditable' }
	];

	constructor(
		private http: _HttpClient,
		private msgSrv: NzMessageService,
		private modalSrv: NzModalService,
		private cdr: ChangeDetectorRef
	) {}

	ngOnInit() {
		this.load();
	}

	load(): void {
		this.loading = true;
		this.http
			.get('https://localhost:5005/api/resource/identity')
			.pipe(tap(() => (this.loading = false)))
			.subscribe((resp) => {
				this.resources = resp;
				this.cdr.detectChanges();
			});
	}

	add(tpl: TemplateRef<{}>) {
		// this.modalSrv.create({
		// 	nzTitle: '新建规则',
		// 	nzContent: tpl,
		// 	nzOnOk: () => {
		// 		this.loading = true;
		// 		this.http.post('/rule', { description: this.description }).subscribe(() => this.getData());
		// 	}
		// });
	}
}
