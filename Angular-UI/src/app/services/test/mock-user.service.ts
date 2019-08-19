import { Injectable } from '@angular/core';
import { Response } from '@angular/http';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from '../../models/user';
import { map, catchError } from 'rxjs/operators';
import { BaseService } from '../shared/base.service';
import { of } from 'rxjs';

@Injectable()
export class MockUserService {
    userList: Array<User> = new Array<User>();

    constructor() {
        let user1: User = new User();
        user1.firstName = "Aravind";
        user1.lastName = "Murugaiyan",
            user1.id = 1,
            user1.employeeId = "AP1";
        this.userList.push(user1);
    }

    getUser(): Observable<User[]> {
        console.log("Mock user service invoked");
        return of(this.userList);
    }

    getSortedUser(propertyName: string, sortAsc: boolean): Observable<User[]> {
        return of(this.userList);
    }

    addUser(user: User): Observable<User> {
        console.log("Mock add user invoked");
        var maxId = Math.max(...this.userList.map(function (o) { return o.id; }, 0));
        user.id = maxId + 1;
        this.userList.push(user);
        console.log(this.userList);
        return of(user);
    }

    updateUser(user: User): Observable<any> {
        console.log("Mock update user invoked");
        console.log(this.userList);
        var u = this.userList.find(usr => {
            return usr.id === user.id;
        });
        console.log(u);
        if (u != undefined && u != null) {
            u.firstName = user.firstName;
            u.lastName = user.lastName;
            u.employeeId = user.employeeId;
            console.log("mock user data updated")
        }
        return of(u);
    }

    deleteUser(user: User): Observable<any> {
        var isDeleted = false;
        console.log("Mock delete user invoked");
        console.log(this.userList);
        var u = this.userList.find(usr => {
            return usr.id === user.id;
        });
        console.log(u);
        if (u != undefined && u != null) { 
            var idx = this.userList.indexOf(u);
            this.userList.splice(idx, 1);   
            isDeleted = true;       
            console.log(this.userList);
        }
        return of(isDeleted);
    }
}   