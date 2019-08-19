import { Injectable } from '@angular/core';
import { Response } from '@angular/http';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from '../models/user';
import { map, catchError } from 'rxjs/operators';
import { BaseService } from './shared/base.service';

@Injectable()
export class UserService extends BaseService {
    constructor(private http: HttpClient) {
        super();
    }

    getUser(): Observable<User[]> {
        console.log("Get Users Invoked");
        return this.http.get<User[]>(super.baseurl() + 'api/users')
            .pipe(catchError(this.handleError));
    }

    getSortedUser({ propertyName, sortAsc }: { propertyName: string; sortAsc: boolean; }): Observable<User[]> {
        console.log("Get Sorted Users Invoked " + propertyName + " " + sortAsc);
        return this.http.get<User[]>(super.baseurl() + 'api/users/' + propertyName + '/' + (sortAsc ? 'asc' : 'desc'))
            .pipe(catchError(this.handleError));
    }

    addUser(user: User): Observable<any> {
        console.log("Add User Invoked");
        return this.http.post(super.baseurl() + 'api/user/add', user)
            .pipe(map((res: Response) => {
                const data = res["data"];
                return data;
            }))
            .pipe(catchError(this.handleError));
    }

    updateUser(user: User): Observable<any> {
        console.log("Update User Invoked");
        return this.http.post(super.baseurl() + 'api/user/update', user)
            .pipe(map((res: Response) => {
                const data = res["data"];
                return data;
            }))
            .pipe(catchError(this.handleError));
    }

    deleteUser(user: User): Observable<any> {
        console.log("Delete User Invoked");
        return this.http.post(super.baseurl() + 'api/user/delete', user)
            .pipe(map((res: Response) => {
                const data = res["data"];
                return data;
            }))
            .pipe(catchError(this.handleError));
    }
}   