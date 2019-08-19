import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { LoaderService } from "../../../services/shared/loader.service";
import { ProjectService } from "../../../services/project.service";
import { Project } from "../../../models/project";

@Component({
  selector: 'app-search-projects',
  templateUrl: './search-projects.component.html',
  styleUrls: ['./search-projects.component.css']
})
export class SearchProjectsComponent implements OnInit {

  sortStartDateAsc: boolean = undefined;
  sortEndDateAsc: boolean = undefined;
  sortPriorityAsc: boolean = undefined;
  sortedStatusAsc: boolean = undefined;

  @Output()
  editSelectedProject = new EventEmitter<Project>();

  projectList: Array<Project>;

  constructor(private loaderService: LoaderService, private projectService: ProjectService) { }

  ngOnInit() {
    this.loadProjects();
  }

  loadProjects() {
    this.loaderService.showLoading(true);
    this.projectService.getProject().subscribe((projList) => {
      console.log("project list ")
      console.log(projList);
      this.projectList = projList;
      this.loaderService.showLoading(false);
    },
      (error) => {   
        console.log("Error caught");
        console.log(error);
        this.loaderService.showError(error);
        this.loaderService.showLoading(false);
      });
  }

  editProject(project: Project) {
    console.log("Edit project Invoked");
    console.log(project);
    this.editSelectedProject.emit(project);
  }

  loadProjectList() {
    this.loadProjects();
  }

  deleteProject(project) {
    let isDeleted: boolean;
    this.loaderService.showLoading(true);
    this.projectService.deleteProject(project).subscribe((data) => {
      this.loaderService.showSuccess('Project suspended successfully')
      this.loadProjectList();
      this.loaderService.showLoading(false);
      isDeleted = data;
    },
      (error) => {
        this.loaderService.showError(error);
        this.loaderService.showLoading(false);
      });
      return isDeleted;
  }

  loadSortedProjects(propertyName: string, sortAsc: boolean) {
    this.loaderService.showLoading(true);
    this.projectService.getSortedProjects(propertyName, sortAsc).subscribe((projList) => {
      this.projectList = projList;
      this.loaderService.showLoading(false);
    },
      (error) => {
        this.loaderService.showError(error);
        this.loaderService.showLoading(false);
      });
  }


  sortProjectsByStartDate() {
    if (this.sortStartDateAsc === undefined || this.sortStartDateAsc === false) {
      this.sortStartDateAsc = true;
    }
    else {
      this.sortStartDateAsc = false;
    }
    this.sortEndDateAsc = undefined;
    this.sortPriorityAsc = undefined;
    this.sortedStatusAsc = undefined;
    this.loadSortedProjects('startDate', this.sortStartDateAsc);
  }

  sortProjectsByEndDate() {
    if (this.sortEndDateAsc === undefined || this.sortEndDateAsc === false) {
      this.sortEndDateAsc = true;
    }
    else {
      this.sortEndDateAsc = false;
    }
    this.sortStartDateAsc = undefined;
    this.sortPriorityAsc = undefined;
    this.sortedStatusAsc = undefined;
    this.loadSortedProjects('endDate', this.sortEndDateAsc);
  }

  sortProjectsByPriority() {
    if (this.sortPriorityAsc === undefined || this.sortPriorityAsc === false) {
      this.sortPriorityAsc = true;
    }
    else {
      this.sortPriorityAsc = false;
    }
    this.sortStartDateAsc = undefined;
    this.sortEndDateAsc = undefined;
    this.sortedStatusAsc = undefined;
    this.loadSortedProjects('priority', this.sortPriorityAsc);
  }

  sortProjectsByStatus() {
    if (this.sortedStatusAsc === undefined || this.sortedStatusAsc === false) {
      this.sortedStatusAsc = true;
    }
    else {
      this.sortedStatusAsc = false;
    }
    this.sortStartDateAsc = undefined;
    this.sortEndDateAsc = undefined;
    this.sortPriorityAsc = undefined;
    this.loadSortedProjects('status', this.sortedStatusAsc);
  }

}
