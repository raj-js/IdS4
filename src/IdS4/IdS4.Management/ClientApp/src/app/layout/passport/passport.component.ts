import { Component } from '@angular/core';
import { SettingsService, App } from '@delon/theme';

@Component({
	selector: 'layout-passport',
	templateUrl: './passport.component.html',
	styleUrls: [ './passport.component.less' ]
})
export class LayoutPassportComponent {
	app: App;
	links = [];

	constructor(private settingsSrv: SettingsService) {
		this.app = this.settingsSrv.app;
	}
}
