import { Component, OnInit } from '@angular/core';
import { _HttpClient } from '@delon/theme';

@Component({
	selector: 'app-dashboard',
	templateUrl: './dashboard.component.html',
	styleUrls: [ './dashboard.component.less' ]
})
export class DashboardComponent implements OnInit {
	constructor(private http: _HttpClient) {}

	ngOnInit() {}
}
