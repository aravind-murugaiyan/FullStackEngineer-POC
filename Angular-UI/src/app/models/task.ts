import { User } from "./user";
import { Project } from './project';

export class Task {
    public id: number;
    public project: Project;
    public name: string;
    public isParentTask: boolean = false;
    public priority: number = 0;
    public parentTask: Task;
    public startDate: Date;
    public endDate: Date;
    public assignedToUser: User ;
    public projectId: number;
    public assignedToUserId: number;
    public parentTaskId: number;
}