import { Component, OnInit, TemplateRef } from '@angular/core';
import { Task } from '../../../models/task';
import { LoaderService } from '../../../services/shared/loader.service';
import { TaskService } from '../../../services/task.services';
import { ProjectService } from '../../../services/project.service';
import { UserService } from '../../../services/user.service';
import { Project } from '../../../models/project';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { User } from '../../../models/user';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-add-task',
  templateUrl: './add-task.component.html',
  styleUrls: ['./add-task.component.css']
})
export class AddTaskComponent implements OnInit {

  data: Task = new Task();
  searchText: string;
  projectList: Array<Project>;
  userList: Array<User>;
  taskList: Array<Task>;
  modalRef: BsModalRef;
  isEditMode: boolean = false;

  constructor(private eventService: LoaderService, private taskService: TaskService, private projectService: ProjectService, private userService: UserService, private modalService: BsModalService, private route: ActivatedRoute) { }

  ngOnInit() {
    console.log("Add task loading");
    console.log(this.data);

    if (this.route.snapshot.params['task']) {
      let taskData = JSON.parse(this.route.snapshot.params['task']);
      if (taskData != undefined && taskData != null) {
        if (taskData.startDate != undefined && taskData.startDate != null) {
          taskData.startDate = taskData.startDate.split("T")[0];
        }

        if (taskData.endDate != undefined && taskData.endDate != null) {
          taskData.endDate = taskData.endDate.split("T")[0];
        }
        this.isEditMode = true;
      }
      this.data = taskData;
      console.log("Fetched task");
      console.log(this.data);
    }
  }

  addTask() {
    let newTask:Task = new Task();
    if (this.validateTaskData(this.data)) {
      this.eventService.showLoading(true);
      if (this.data.isParentTask) {
        this.data.startDate = undefined;
        this.data.endDate = undefined;
        this.data.priority = 0;
        this.data.assignedToUserId = undefined;
        this.data.parentTask = undefined;
      }
      console.log(this.data);
      this.taskService.addTask(this.data).subscribe((data) => {
        this.eventService.showSuccess('Saved successfully');
        this.data = new Task();
        this.eventService.showLoading(false);
        console.log("response task");
        console.log(data);
        newTask = data;
      },
        (error) => {
          this.eventService.showError(error);
          this.eventService.showLoading(false);
        });
    }
    return newTask;
  }

  updateTask() {
    let updatedTask:Task;
    if (this.validateTaskData(this.data)) {
      this.eventService.showLoading(true);
      this.taskService.updateTask(this.data).subscribe((data) => {
        this.eventService.showSuccess("Task updated sucessfully.");
        this.eventService.showLoading(false);
        this.isEditMode = false;
        this.resetTask();
        updatedTask = data;
        console.log("response update task");
        console.log(data);
      },
        (error) => {
          this.eventService.showError(error);
          this.eventService.showLoading(false);
        }
      );
    }
    return updatedTask;
  }

  resetTask() {
    this.data = new Task();
    this.isEditMode = false;
  }

  openModal(template: TemplateRef<any>, type: number) {
    this.searchText = undefined;
    if (type === 1) {
      this.eventService.showLoading(true);
      this.projectService.getProject().subscribe((projects) => {
        this.projectList = projects;
        console.log(this.projectList);
        this.modalRef = this.modalService.show(template);
        document.getElementsByTagName("modal-container")[0].classList.remove("fade");
        this.eventService.showLoading(false);
      },
        (error) => {
          this.eventService.showError(error);
          this.eventService.showLoading(false);
        });
    }
    if (type === 2) {
      if (!this.data.isParentTask) {
        this.eventService.showLoading(true);
        this.taskService.getTasks().subscribe((tasks) => {
          console.log("Tasks retreived");
          this.taskList = tasks;
          console.log(tasks);
          this.modalRef = this.modalService.show(template);
          document.getElementsByTagName("modal-container")[0].classList.remove("fade");
          this.eventService.showLoading(false);
        },
          (error) => {
            this.eventService.showError(error);
            this.eventService.showLoading(false);
          });
      } else {
        this.eventService.showWarning('parent task only needs task name and project');
      }
    }
    if (type === 3) {
      if (!this.data.isParentTask) {
        this.eventService.showLoading(true);
        this.userService.getUser().subscribe((user) => {
          this.userList = user;
          this.modalRef = this.modalService.show(template);
          document.getElementsByTagName("modal-container")[0].classList.remove("fade");
          this.eventService.showLoading(false);
        },
          (error) => {
            this.eventService.showError(error);
            this.eventService.showLoading(false);
          });
      } else {
        this.eventService.showWarning('parent task only needs task name and project');
      }
    }
  }

  selectProject(project: Project) {
    this.data.project = project;
    this.data.projectId = project.id;
    this.modalRef.hide();
  }

  selectUser(user: User) {
    this.data.assignedToUser = user;
    this.data.assignedToUserId = user.id;
    this.modalRef.hide();
    console.log(user);
  }

  selectParentTask(task: Task) {
    this.data.parentTask = task;
    this.data.parentTaskId = task.id;;
    console.log(this.data)
    this.modalRef.hide();
  }

  validateTaskData(task: Task) {
    let isValid: boolean = true;
    if (task.project === undefined) {
      this.eventService.showWarning("Select a project.");
      isValid = false;
    }

    if (task.name === undefined) {
      this.eventService.showWarning("Enter the task name.");
      isValid = false;
    }

    if (!task.isParentTask) {
      if (task.startDate === undefined) {
        this.eventService.showWarning("Select the start date");
        isValid = false;
      }

      if (task.endDate === undefined) {
        this.eventService.showWarning("Select the end date");
        isValid = false;
      }
    }

    return isValid;
  }
}
