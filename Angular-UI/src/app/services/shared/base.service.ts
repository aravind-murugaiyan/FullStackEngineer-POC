import { Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { ServiceError } from '../../models/serviceerror';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';
import { HttpErrorResponse } from '@angular/common/http';


export class BaseService {
    constructor() { }
    protected extractData(res: Response) {
        const body = res.json();
        if (body.status === 'success') {
            return body.data;
        } else if (body.status === 'fail') {
            throw new ServiceError(body.message, body.data, 'fail');
        } else if (body.status === 'error') {
            throw new ServiceError(body.message, body.data);
        } else {
            throw new ServiceError('Invalid JSend Response Status [' + body.status + ']');
        }
    }
    public baseurl(): string {
        return 'http://localhost:50830/';
    }

    protected handleError(error: any) {

        console.log("Error")
        console.log(error);
        console.log(error.status + ":" + error.statusText);
        console.log(error.error.message);
        if (error instanceof HttpErrorResponse) {
            return Observable.throw("[" + error.status + ":" + error.statusText + "] " + (error.error != undefined && error.error.message != undefined ? error.error.message : ''));
        }
        else {
            return Observable.throw(error);
        }
    }
}