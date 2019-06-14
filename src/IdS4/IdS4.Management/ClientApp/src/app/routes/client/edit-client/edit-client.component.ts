import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { SFSchema } from '@delon/form';
import { NzMessageService } from 'ng-zorro-antd';
import { ConfigurationService } from '@shared/services/configuration.service';
import { _HttpClient } from '@delon/theme';
import { Router, ActivatedRoute } from '@angular/router';
import { IApiResult } from '@shared/models/api-result.model';
import { ApiResultCode } from '@shared/models/api-result-code.enum';
import { Observable } from 'rxjs';
import { map, filter } from 'rxjs/operators';

@Component({
	selector: 'app-edit-client',
	templateUrl: './edit-client.component.html',
	styles: []
})
export class EditClientComponent implements OnInit {
	url: string;
	url$: Observable<string>;
	id: number;

	loadingBasicForm = false;
	loadingAuthenticateForm = false;
	loadingTokenForm = false;
	loadingConsentForm = false;
	loadingDeviceForm = false;

	basicForm: any;
	authenticateForm: any;
	tokenForm: any;
	consentForm: any;
	deviceForm: any;

	basicFormSchema: SFSchema = {
		properties: {
			id: { type: 'number', ui: { hidden: true } },
			clientId: { type: 'string', title: '客户端标识', readOnly: true },
			clientName: { type: 'string', title: '客户端名称', maxLength: 32 },
			enabled: { type: 'boolean', title: '是否启用', default: true },
			protocolType: {
				type: 'string',
				title: '协议类型',
				enum: [ { label: 'OpenId Connect', value: 'oidc' } ],
				ui: { widget: 'select' },
				default: 'oidc'
			},
			requireClientSecret: { type: 'boolean', title: '必须客户端密码', default: false },
			clientSecrets: {
				type: 'array',
				title: '密码',
				items: {
					type: 'object',
					properties: {
						id: { title: 'ID', type: 'number', default: 0, ui: { widget: 'text' } },
						clientId: { type: 'number', ui: { hidden: true } },
						type: {
							title: '类型',
							type: 'string',
							maxLength: 256,
							enum: [
								{ label: 'SharedSecret', value: 'SharedSecret' },
								{ label: 'X509Thumbprint', value: 'X509Thumbprint' },
								{ label: 'X509Name', value: 'X509Name' },
								{ label: 'X509CertificateBase64', value: 'X509CertificateBase64' }
							],
							ui: {
								widget: 'select'
							},
							default: 'SharedSecret'
						},
						value: {
							title: '值',
							type: 'string',
							maxLength: 4000,
							ui: { widget: 'textarea', autosize: { minRows: 2, maxRows: 3 } }
						},
						description: {
							title: '描述',
							type: 'string',
							maxLength: 1000,
							ui: { widget: 'textarea', autosize: { minRows: 2, maxRows: 3 } }
						},
						expiration: { title: '过期', type: 'string', format: 'date' }
					},
					required: [ 'value' ]
				}
			},
			allowedGrantTypes: {
				type: 'array',
				title: '允许的授权类型',
				items: {
					type: 'object',
					properties: {
						id: { title: 'ID', type: 'number', default: 0, ui: { widget: 'text' } },
						clientId: { type: 'number', ui: { hidden: true } },
						grantType: {
							title: '授权类型',
							type: 'string',
							enum: [
								{ label: 'implicit', value: 'implicit' },
								{ label: 'hybrid', value: 'hybrid' },
								{ label: 'authorization code', value: 'authorization_code' },
								{ label: 'client credentials', value: 'client_credentials' },
								{ label: 'resource owner password', value: 'password' },
								{ label: 'device flow', value: 'urn:ietf:params:oauth:grant-type:device_code' }
							]
						}
					},
					required: [ 'grantType' ]
				}
			},
			requirePkce: { type: 'boolean', title: '必须发送校验密钥', default: false },
			allowPlainTextPkce: { type: 'boolean', title: '允许纯文本校验密钥质询', default: false },
			redirectUris: {
				type: 'array',
				title: '跳转路径',
				items: {
					type: 'object',
					properties: {
						id: { title: 'ID', type: 'number', default: 0, ui: { widget: 'text' } },
						clientId: { type: 'number', ui: { hidden: true } },
						redirectUri: { type: 'string', maxLength: 2000, title: '路径' }
					},
					required: [ 'redirectUri' ]
				}
			},
			allowedScopes: {
				type: 'array',
				title: '允许范围',
				items: {
					type: 'object',
					properties: {
						id: { title: 'ID', type: 'number', default: 0, ui: { widget: 'text' } },
						clientId: { type: 'number', ui: { hidden: true } },
						scope: {
							type: 'string',
							title: '范围',
							ui: {
								widget: 'select',
								asyncData: () =>
									this.http
										.get(`${this.url}/api/scope`)
										.pipe(
											map((resp: any) => resp as IApiResult),
											filter((res: IApiResult) => res.code === ApiResultCode.Success),
											map((res: IApiResult) => res.data)
										)
							}
						}
					},
					required: [ 'scope' ]
				}
			},
			allowOfflineAccess: { type: 'boolean', title: '允许离线访问权限', default: false },
			allowAccessTokensViaBrowser: { type: 'boolean', title: '允许通过浏览器获取 Token', default: false },
			properties: {
				title: '资源属性',
				type: 'array',
				items: {
					type: 'object',
					properties: {
						id: { title: 'ID', type: 'number', default: 0, ui: { widget: 'text' } },
						apiResourceId: { type: 'number', ui: { hidden: true } },
						key: { title: '键', type: 'string', maxLength: 32 },
						value: { title: '值', type: 'string', maxLength: 256 }
					},
					required: [ 'key' ]
				}
			},
			description: {
				type: 'string',
				title: '描述',
				maxLength: 256,
				ui: {
					widget: 'textarea',
					autosize: {
						minRows: 2,
						maxRows: 3
					}
				}
			}
		},
		required: [ 'clientId', 'clientName' ]
	};
	authenticateFormSchema: SFSchema = {
		properties: {
			id: { type: 'number', ui: { hidden: true } },
			postLogoutRedirectUris: {
				type: 'array',
				title: '注销后跳转路径',
				items: {
					type: 'object',
					properties: {
						id: { title: 'ID', type: 'number', default: 0, ui: { widget: 'text' } },
						clientId: { type: 'number', ui: { hidden: true } },
						postLogoutRedirectUri: { type: 'string', maxLength: 2000, title: '路径' }
					},
					required: [ 'postLogoutRedirectUri' ]
				}
			},
			frontChannelLogoutUri: { type: 'string', maxLength: 2000, title: '前端通道注销路径' },
			frontChannelLogoutSessionRequired: { type: 'boolean', title: 'Session Id是否传递到前端通道注销路径', default: true },
			backChannelLogoutUri: { type: 'string', maxLength: 2000, title: '后端通道注销路径' },
			backChannelLogoutSessionRequired: { type: 'boolean', title: 'Session Id是否传递到后端通道注销路径', default: true },
			enableLocalLogin: { type: 'boolean', title: '启用本地登录', default: true },
			identityProviderRestrictions: {
				type: 'array',
				title: 'IdPs',
				items: {
					type: 'object',
					properties: {
						id: { title: 'ID', type: 'number', default: 0, ui: { widget: 'text' } },
						clientId: { type: 'number', ui: { hidden: true } },
						provider: { type: 'string', maxLength: 200, title: '提供者' }
					},
					required: [ 'provider' ]
				}
			},
			userSsoLifetime: { type: 'number', title: '用户认证有效时长', minimum: 0, ui: { unit: '秒' } }
		}
	};
	tokenFormSchema: SFSchema = {
		properties: {
			id: { type: 'number', ui: { hidden: true } },
			identityTokenLifetime: { type: 'number', title: '身份令牌有效时长', minimum: 0, default: 300, ui: { unit: '秒' } },
			accessTokenLifetime: { type: 'number', title: '访问令牌有效时长', minimum: 0, default: 3600, ui: { unit: '秒' } },
			authorizationCodeLifetime: {
				type: 'number',
				title: '认证代码有效时长',
				minimum: 0,
				default: 300,
				ui: { unit: '秒' }
			},
			absoluteRefreshTokenLifetime: {
				type: 'number',
				title: '刷新令牌有效时长（绝对）',
				minimum: 0,
				default: 2592000,
				ui: { unit: '秒' }
			},
			slidingRefreshTokenLifetime: {
				type: 'number',
				title: '刷新令牌有效时长（弹性）',
				minimum: 0,
				default: 1296000,
				ui: { unit: '秒' }
			},
			refreshTokenUsage: {
				type: 'integer',
				title: '刷新令牌用法',
				enum: [ { label: '可复用', value: 0 }, { label: '一次性', value: 1 } ],
				default: 1,
				ui: { widget: 'select' }
			},
			refreshTokenExpiration: {
				type: 'integer',
				title: '刷新令牌过期方式',
				enum: [ { label: '绝对过期', value: 0 }, { label: '弹性过期', value: 1 } ],
				default: 1,
				ui: { widget: 'select' }
			},
			updateAccessTokenClaimsOnRefresh: { type: 'boolean', title: '请求刷新令牌时更新访问令牌声明' },
			accessTokenType: {
				type: 'integer',
				title: '访问令牌类型',
				enum: [ { label: 'JWT', value: 0 }, { label: 'ReferenceToken', value: 1 } ],
				default: 1,
				ui: { widget: 'select' }
			},
			includeJwtId: { type: 'boolean', title: 'JWT是否需要嵌入唯一ID' },
			allowedCorsOrigins: {
				type: 'array',
				title: '允许跨域访问的源',
				items: {
					type: 'object',
					properties: {
						id: { title: 'ID', type: 'number', default: 0, ui: { widget: 'text' } },
						clientId: { type: 'number', ui: { hidden: true } },
						origin: { type: 'string', maxLength: 150, title: '源URI' }
					},
					required: [ 'origin' ]
				}
			},
			claims: {
				title: '客户端声明',
				type: 'array',
				items: {
					type: 'object',
					properties: {
						id: { title: 'ID', type: 'number', default: 0, ui: { widget: 'text' } },
						apiResourceId: { type: 'number', ui: { hidden: true } },
						type: { title: '类型', type: 'string', maxLength: 250 },
						value: { title: '值', type: 'string', maxLength: 250 }
					},
					required: [ 'type', 'value' ]
				}
			},
			alwaysSendClientClaims: { type: 'boolean', title: '总是发送声明在任何流程中', default: false },
			alwaysIncludeUserClaimsInIdToken: { type: 'boolean', title: '总是在请求IdToken时返回用户声明', default: false },
			clientClaimsPrefix: { type: 'string', title: '客户端声明前缀', default: 'client_' },
			pairWiseSubjectSalt: { type: 'string', title: 'PairWiseSubjectSalt' }
		}
	};
	consentFormSchema: SFSchema = {
		properties: {
			id: { type: 'number', ui: { hidden: true } },
			requireConsent: { type: 'boolean', title: '必须用户授权', default: true },
			allowRememberConsent: { type: 'boolean', title: '允许记住用户授权', default: true },
			consentLifetime: {
				type: 'number',
				title: '用户授权有效时间',
				minimum: 0,
				ui: { unit: '秒' }
			},
			clientUri: { type: 'string', title: '客户端地址' },
			logoUri: { type: 'string', title: '客户端logo地址' }
		}
	};
	deviceFormSchema: SFSchema = {
		properties: {
			id: { type: 'number', ui: { hidden: true } },
			userCodeType: { type: 'string', title: '用户代码类型' },
			deviceCodeLifetime: {
				type: 'number',
				title: '设备代码有效时长',
				minimum: 0,
				default: 300,
				ui: { unit: '秒' }
			}
		}
	};

	constructor(
		private msgSrv: NzMessageService,
		private configSrv: ConfigurationService,
		private http: _HttpClient,
		private cdr: ChangeDetectorRef,
		private router: Router,
		private route: ActivatedRoute
	) {}

	ngOnInit() {
		this.id = Number.parseInt(this.route.snapshot.paramMap.get('id'), 10);

		if (this.id === Number.NaN) {
			this.msgSrv.error('客户端ID无效！');
			return;
		}

		if (this.configSrv.isReady) {
			this.url = this.configSrv.serverSettings.coreApiUrl;
			this.load();
		} else {
			this.configSrv.settingsLoaded$.subscribe(() => {
				this.url = this.configSrv.serverSettings.coreApiUrl;
				this.load();
			});
		}
	}

	load(): void {
		this.loadingBasicForm = true;
		this.http.get(`${this.url}/api/client/${this.id}`).subscribe((resp) => {
			this.loadingBasicForm = false;
			const result = resp as IApiResult;
			if (result.code === ApiResultCode.Success) {
				const d = result.data;
				this.basicForm = d.basic;
				this.authenticateForm = d.authenticate;
				this.tokenForm = d.token;
				this.consentForm = d.consent;
				this.deviceForm = d.device;
			} else {
				this.msgSrv.error(`code: ${result.code} \r\n errors: ${JSON.stringify(result.errors)}`);
			}
			this.cdr.detectChanges();
		});
	}

	submitBasicForm(value: any): void {
		this.loadingBasicForm = true;
		this.http.patch(`${this.url}/api/client/basic`, value).subscribe((resp) => {
			this.loadingBasicForm = false;
			const result = resp as IApiResult;
			if (result.code === ApiResultCode.Success) {
				this.basicForm = result.data;
				this.msgSrv.success('操作成功');
			} else {
				this.msgSrv.error(`code: ${result.code} \r\n errors: ${JSON.stringify(result.errors)}`);
			}
			this.cdr.detectChanges();
		});
	}
	submitAuthenticateForm(value: any): void {
		this.loadingAuthenticateForm = true;
		this.http.patch(`${this.url}/api/client/authenticate`, value).subscribe((resp) => {
			this.loadingAuthenticateForm = false;
			const result = resp as IApiResult;
			if (result.code === ApiResultCode.Success) {
				this.authenticateForm = result.data;
				this.msgSrv.success('操作成功');
			} else {
				this.msgSrv.error(`code: ${result.code} \r\n errors: ${JSON.stringify(result.errors)}`);
			}
			this.cdr.detectChanges();
		});
	}
	submitTokenForm(value: any): void {
		this.loadingTokenForm = true;
		this.http.patch(`${this.url}/api/client/token`, value).subscribe((resp) => {
			this.loadingTokenForm = false;
			const result = resp as IApiResult;
			if (result.code === ApiResultCode.Success) {
				this.tokenForm = result.data;
				this.msgSrv.success('操作成功');
			} else {
				this.msgSrv.error(`code: ${result.code} \r\n errors: ${JSON.stringify(result.errors)}`);
			}
			this.cdr.detectChanges();
		});
	}
	submitConsentForm(value: any): void {
		this.loadingConsentForm = true;
		this.http.patch(`${this.url}/api/client/consent`, value).subscribe((resp) => {
			this.loadingConsentForm = false;
			const result = resp as IApiResult;
			if (result.code === ApiResultCode.Success) {
				this.consentForm = result.data;
				this.msgSrv.success('操作成功');
			} else {
				this.msgSrv.error(`code: ${result.code} \r\n errors: ${JSON.stringify(result.errors)}`);
			}
			this.cdr.detectChanges();
		});
	}
	submitDeviceForm(value: any): void {
		this.loadingDeviceForm = true;
		this.http.patch(`${this.url}/api/client/device`, value).subscribe((resp) => {
			this.loadingDeviceForm = false;
			const result = resp as IApiResult;
			if (result.code === ApiResultCode.Success) {
				this.deviceForm = result.data;
				this.msgSrv.success('操作成功');
			} else {
				this.msgSrv.error(`code: ${result.code} \r\n errors: ${JSON.stringify(result.errors)}`);
			}
			this.cdr.detectChanges();
		});
	}
}
