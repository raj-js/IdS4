import { Injectable } from '@angular/core';
import { IConfiguration } from '@shared/models/configuration.model';
import { Subject } from 'rxjs';
import { StorageService } from './storage.service';
import { HttpClient } from '@angular/common/http';

@Injectable({
	providedIn: 'root'
})
export class ConfigurationService {
	serverSettings: IConfiguration;
	private settingsLoadedSource = new Subject();
	settingsLoaded$ = this.settingsLoadedSource.asObservable();
	isReady = false;

	constructor(private http: HttpClient, private storageService: StorageService) {}

	load() {
		const url = `${window.location.origin}/api/Settings`;
		this.http.get(url).subscribe((response) => {
			this.serverSettings = response as IConfiguration;
			console.log(this.serverSettings);

			this.storageService.store('CoreApiUrl', this.serverSettings.coreApiUrl);
			this.isReady = true;
			this.settingsLoadedSource.next();
		});
	}
}
