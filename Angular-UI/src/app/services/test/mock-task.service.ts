import { Injectable } from '@angular/core';
import { Response } from '@angular/http';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { Task } from '../../models/task';
import { map, catchError } from 'rxjs/operators';
import { BaseService } from '../shared/base.service';

@Injectable()
export class MockTaskService {
    taskList: Array<Task> = new Array<Task>();

    constructor() {
        var task1 = new Task();
        task1.id=1;
        task1.name="Task 1";
        task1.assignedToUserId = 1;
        task1.isParentTask=true;
        task1.projectId=1;
        this.taskList.push(task1);

        var task2 = new Task();
        task2.id=2;
        task2.name="Task 1";
        task2.assignedToUserId = 1;
        task2.isParentTask=true;
        task2.projectId=1;
        this.taskList.push(task2);
    }

    getTasks(): Observable<Task[]> {
        console.log("Mock Get All Tasks Invoked");
        return of(this.taskList);
    }

    addTask(task: Task): Observable<any> {
        console.log("Mock add task invoked");
        var maxId = Math.max(...this.taskList.map(function (o) { return o.id; }, 0));
        task.id = maxId + 1;
        this.taskList.push(task);
        console.log(this.taskList);
        return of(task);
    }

    getAllTasksByProjectId(projectId: number): Observable<Task[]> {
        console.log("Mock get all task by project id "+projectId);
        var t = this.taskList.filter(proj => {
            return proj.projectId === projectId
        });
        return of(this.taskList);
    }

    updateTask(task: Task): Observable<any> {
        console.log("Mock update task invoked");
        console.log(this.taskList);
        var t = this.taskList.find(tsk => {
            return tsk.id === task.id;
        });
        console.log(t);
        if (t != undefined && t != null) {
            t.name = task.name;
            t.startDate=task.startDate;
            t.endDate=task.endDate;
            t.priority=task.priority;
            t.parentTaskId = task.parentTaskId;
            t.parentTaskId = task.parentTaskId;
            t.assignedToUserId=task.assignedToUserId;
            console.log("mock task data updated")
        }
        return of(t);
    }

    deleteTask(task: Task): Observable<any> {
        console.log("Mock delete task invoked");
        var isDeleted = false;
        console.log(this.taskList);
        var t = this.taskList.find(proj => {
            return proj.id === task.id;
        });
        console.log(t);
        if (t != undefined && t != null) { 
            var idx = this.taskList.indexOf(t);
            this.taskList.splice(idx, 1);   
            isDeleted = true;       
            console.log(this.taskList);
        }
        return of(isDeleted);
    }


    getSortedTasks(projectId: number, propertyName: string, sortAsc: boolean): Observable<Task[]> {
        console.log("Mock get sorted task list");
        return of(this.taskList);
    }
}   