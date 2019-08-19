import { Injectable } from '@angular/core';
import { Response } from '@angular/http';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Task } from '../models/task';
import { map, catchError } from 'rxjs/operators';
import { BaseService } from './shared/base.service';

@Injectable()
export class TaskService extends BaseService {
    constructor(private http: HttpClient) {
        super();
    }

    getTasks(): Observable<Task[]> {
        console.log("Fetching Task List");
        return this.http.get<Task[]>(super.baseurl() + 'api/tasks')
            .pipe(map((res) => {
                console.log("Task List Received");
                console.log(res);
                return res;
            }))
            .pipe(catchError(this.handleError));
    }
    addTask(task: Task): Observable<any> {
        console.log("Add task invoked");
        console.log(task);
        return this.http.post(super.baseurl() + 'api/task/add', task)
            .pipe(map((res: Response) => {
                const data = res["data"];
                return data;
            }))
            .pipe(catchError(this.handleError));
    }

    getAllTasksByProjectId(projectId: number): Observable<Task[]> {
        console.log("Fetching all task by project id.")
        return this.http.get<Task[]>(super.baseurl() + 'api/task/project/' + projectId)
            .pipe(map((res) => {
                console.log("Task List by project id received");
                console.log(res);
                return res;
            }))
            .pipe(catchError(this.handleError));
    }

    updateTask(task: Task): Observable<any> {
        return this.http.post(super.baseurl() + 'api/task/update', task)
            .pipe(map((res: Response) => {
                const data = res["data"];
                return data;
            }))
            .pipe(catchError(this.handleError));
    }

    deleteTask(task: Task): Observable<any> {
        return this.http.post(super.baseurl() + 'api/task/delete', task)
            .pipe(map((res: Response) => {
                const data = res["data"];
                return data;
            }))
            .pipe(catchError(this.handleError));
    }


    getSortedTasks(projectId: number, propertyName: string, sortAsc: boolean): Observable<Task[]> {
        return this.http.get<Task[]>(super.baseurl() + 'api/tasks/sort/' + projectId + '/' + propertyName + "/" + (sortAsc ? 'asc' : 'desc'))
            .pipe(map((res) => {
                return res;
            }))
            .pipe(catchError(this.handleError));
    }
}   