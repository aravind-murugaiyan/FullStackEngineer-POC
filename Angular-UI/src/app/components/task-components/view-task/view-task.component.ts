import { Component, OnInit } from '@angular/core';
import { Task } from '../../../models/task';
import { LoaderService } from '../../../services/shared/loader.service';
import { TaskService } from '../../../services/task.services';
import { ProjectService } from '../../../services/project.service';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { Project } from '../../../models/project';
import { Template } from '@angular/compiler/src/render3/r3_ast';
import { Router } from '@angular/router';

@Component({
  selector: 'app-view-task',
  templateUrl: './view-task.component.html',
  styleUrls: ['./view-task.component.css']
})
export class ViewTaskComponent implements OnInit {

  taskList: Array<Task> = new Array<Task>();
  projectList: Array<Project> = new Array<Project>();
  modalRef: BsModalRef;
  selectedProject: Project = new Project();

  sortStartDateAsc: boolean = undefined;
  sortEndDateAsc: boolean = undefined;
  sortPriorityAsc: boolean = undefined;
  sortStatusAsc: boolean = undefined;

  constructor(private loaderService: LoaderService, private taskService: TaskService, private projectService: ProjectService, private modalService: BsModalService, private router: Router) { }

  ngOnInit() {

  }

  openModal(template: Template) {
    this.loaderService.showLoading(true);
    this.projectService.getProject().subscribe((projects) => {
      this.projectList = projects;
      console.log(this.projectList);
      this.modalRef = this.modalService.show(template);
      document.getElementsByTagName("modal-container")[0].classList.remove("fade");
      this.loaderService.showLoading(false);
    },
      (error) => {
        this.loaderService.showError(error);
        this.loaderService.showLoading(false);
      });
  }

  selectProject(project: Project) {
    this.selectedProject = project;
    this.modalRef.hide();
    this.loadTaksByProject(project);
  }

  loadTaksByProject(project: Project) {
    this.loaderService.showLoading(true);
    this.taskService.getAllTasksByProjectId(project.id).subscribe((tasks) => {
      if (tasks != undefined && tasks.length > 0) {
        this.taskList = tasks;
        this.sortStartDateAsc = undefined;
        this.sortEndDateAsc = undefined;
        this.sortPriorityAsc = undefined;
        this.sortStatusAsc = undefined;
      }
      else {
        this.taskList = new Array<Task>();
        this.sortStartDateAsc = undefined;
        this.sortEndDateAsc = undefined;
        this.sortPriorityAsc = undefined;
        this.sortStatusAsc = undefined;
      }
      console.log(tasks);
      this.loaderService.showLoading(false);
    },
      (error) => {
        this.loaderService.showError(error);
        this.loaderService.showLoading(false);
      });
  }

  deleteTask(task) {
    var isDeleted = false;
    this.loaderService.showLoading(true);
    this.taskService.deleteTask(task).subscribe((data) => {
      this.loaderService.showSuccess('Task completed successfully')
      this.loadTaksByProject(this.selectedProject);
      this.loaderService.showLoading(false);
      isDeleted=true;
    },
      (error) => {
        this.loaderService.showError(error);
        this.loaderService.showLoading(false);
      });
      return isDeleted;
  }

  editTask(task) {
    this.router.navigate(['/add-task', { task: JSON.stringify(task) }]);
  }

  isStartDateAsc: boolean = undefined;
  isEndDateAsc: boolean = undefined;

  loadSortedTasks(projectId: number, propertyName: string, sortAsc: boolean) {
    this.loaderService.showLoading(true);
    this.taskService.getSortedTasks(projectId, propertyName, sortAsc).subscribe((tasks) => {
      if (tasks != undefined && tasks.length > 0) {
        this.taskList = tasks;
      }
      else {
        this.taskList = new Array<Task>();
      }
      console.log(tasks);
      this.loaderService.showLoading(false);
    },
      (error) => {
        this.loaderService.showError(error);
        this.loaderService.showLoading(false);
      });
  }


  sortTasksByStartDate() {
    if (this.sortStartDateAsc === undefined || this.sortStartDateAsc === false) {
      this.sortStartDateAsc = true;
    }
    else {
      this.sortStartDateAsc = false;
    }
    this.sortEndDateAsc = undefined;
    this.sortPriorityAsc = undefined;
    this.sortStatusAsc = undefined;
    this.loadSortedTasks(this.selectedProject.id, 'startDate', this.sortStartDateAsc);
  }


  sortTasksByEndDate() {
    if (this.sortEndDateAsc === undefined || this.sortEndDateAsc === false) {
      this.sortEndDateAsc = true;
    }
    else {
      this.sortEndDateAsc = false;
    }
    this.sortStartDateAsc = undefined;
    this.sortPriorityAsc = undefined;
    this.sortStatusAsc = undefined;
    this.loadSortedTasks(this.selectedProject.id, 'endDate', this.sortEndDateAsc);
  }

  sortTasksByPriority() {
    if (this.sortPriorityAsc === undefined || this.sortPriorityAsc === false) {
      this.sortPriorityAsc = true;
    }
    else {
      this.sortPriorityAsc = false;
    }
    this.sortEndDateAsc = undefined;
    this.sortStartDateAsc = undefined;
    this.sortStatusAsc = undefined;
    this.loadSortedTasks(this.selectedProject.id, 'priority', this.sortPriorityAsc);
  }

  sortTasksByStatus() {
    if (this.sortStatusAsc === undefined || this.sortStatusAsc === false) {
      this.sortStatusAsc = true;
    }
    else {
      this.sortStatusAsc = false;
    }
    this.sortStartDateAsc = undefined;
    this.sortEndDateAsc = undefined;
    this.sortPriorityAsc = undefined;
    this.loadSortedTasks(this.selectedProject.id, 'status', this.sortStatusAsc);
  }
}
