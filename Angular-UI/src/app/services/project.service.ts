import { Injectable } from '@angular/core';
import { Response } from '@angular/http';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Project } from '../models/project';
import { map, catchError } from 'rxjs/operators';
import { BaseService } from './shared/base.service';

@Injectable()
export class ProjectService extends BaseService {
    constructor(private http: HttpClient) {
        super();
    }

    getProject(): Observable<Project[]> {
        console.log("Get Projects Invoked");
        return this.http.get<Project[]>(super.baseurl() + 'api/projects').pipe(map((res) => {
            return res;
        }))
            .pipe(catchError(this.handleError));
    }

    getSortedProjects(propertyName: string, sortAsc: boolean): Observable<Project[]> {
        console.log("Get Sorted Users Invoked " + propertyName + " " + sortAsc);
        return this.http.get<Project[]>(super.baseurl() + 'api/projects/sort/' + propertyName + '/' + (sortAsc ? 'asc' : 'desc'))
            .pipe(catchError(this.handleError));
    }

    addProject(project: Project): Observable<any> {
        return this.http.post(super.baseurl() + 'api/projects/add', project)
            .pipe(map((res: Response) => {
                const data = res["data"];
                return data;
            }))
            .pipe(catchError(this.handleError));
    }

    updateProject(project: Project): Observable<any> {
        return this.http.post(super.baseurl() + 'api/projects/update', project)
            .pipe(map((res: Response) => {
                const data = res["data"];
                return data;
            }))
            .pipe(catchError(this.handleError));
    }

    deleteProject(project: Project): Observable<any> {
        return this.http.post(super.baseurl() + 'api/projects/delete/' + project.id, null)
            .pipe(map((res: Response) => {
                const data = res["data"];
                return data;
            }))
            .pipe(catchError(this.handleError));
    }
}   