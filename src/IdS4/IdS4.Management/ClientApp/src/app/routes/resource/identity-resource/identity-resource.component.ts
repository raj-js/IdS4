import { Component, OnInit } from '@angular/core';
import { STColumn } from '@delon/abc';

@Component({
	selector: 'app-identity-resource',
	templateUrl: './identity-resource.component.html',
	styles: []
})
export class IdentityResourceComponent implements OnInit {
	users: any[] = Array(10).fill({}).map((item: any, idx: number) => {
		return {
			id: idx + 1,
			name: `name ${idx + 1}`,
			age: Math.ceil(Math.random() * 10) + 20,
			showExpand: idx !== 0,
			description: `${idx + 1}. My name is John Brown, I am 32 years old, living in New York No.1 Lake Park.`
		};
	});

	columns: STColumn[] = [
		{ title: 'ID', index: 'id' },
		{ title: '姓名', index: 'name' },
		{ title: '年龄', index: 'age' }
	];

	constructor() {}

	ngOnInit() {}
}
