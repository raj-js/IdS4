import { ApiResultCode } from './api-result-code.enum';

export class IApiResult {
	code: ApiResultCode;
	errors: any;
	data: any;
	msg: string | null;
}
