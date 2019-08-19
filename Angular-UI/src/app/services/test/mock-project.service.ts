import { Injectable } from '@angular/core';
import { Response } from '@angular/http';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { Project } from '../../models/project';
import { map, catchError } from 'rxjs/operators';

@Injectable()
export class MockProjectService  {
    projectList: Array<Project> = new Array<Project>();
    constructor() {
        var proj1 = new Project();
        proj1.id=1;
        proj1.managerUserId=1;
        proj1.name="Project 1";
        this.projectList.push(proj1);

        var proj2 = new Project();
        proj2.id=2;
        proj2.managerUserId=1;
        proj2.name="Project 2";
        this.projectList.push(proj2);
    }

    getProject(): Observable<Project[]> {
        console.log("Mock Get All Projects Invoked");
        return of(this.projectList);
    }

    getSortedProjects(propertyName: string, sortAsc: boolean): Observable<Project[]> {
        console.log("Mock Get Sorted Projects invoked");
        return of(this.projectList);
    }

    addProject(project: Project): Observable<any> {
        console.log("Mock add project invoked");
        var maxId = Math.max(...this.projectList.map(function (o) { return o.id; }, 0));
        project.id = maxId + 1;
        this.projectList.push(project);
        console.log(this.projectList);
        return of(project);
    }

    updateProject(project: Project): Observable<any> {
        console.log("Mock update project invoked");
        console.log(this.projectList);
        var p = this.projectList.find(proj => {
            return proj.id === project.id;
        });
        console.log(p);
        if (p != undefined && p != null) {
            p.name = project.name;
            p.managerUserId = project.managerUserId;
            p.startDate=project.startDate;
            p.endDate=project.endDate;
            console.log("mock project data updated")
        }
        return of(p);
    }

    deleteProject(project: Project): Observable<any> {
        console.log("Mock delete project invoked");
        var isDeleted = false;
        console.log(this.projectList);
        var p = this.projectList.find(proj => {
            return proj.id === project.id;
        });
        console.log(p);
        if (p != undefined && p != null) { 
            var idx = this.projectList.indexOf(p);
            this.projectList.splice(idx, 1);   
            isDeleted = true;       
            console.log(this.projectList);
        }
        return of(isDeleted);
    }
}   