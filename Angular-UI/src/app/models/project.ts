import { User } from "./user";

export class Project {
    public id: number;
    public name: string;
    public startDate: string;
    public endDate: string;
    public priority: number = 0;
    public user: User;
    public managerUserId: number;
    public noOfTasks: number = 0;
    public noOfCompletedTasks: number = 0;
    public enableStartAndEndDates: boolean;
    public isCompleted: boolean;
}
