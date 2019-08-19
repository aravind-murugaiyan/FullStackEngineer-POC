import { Component, OnInit, EventEmitter, Output, TemplateRef } from '@angular/core';
import { LoaderService } from "../../../services/shared/loader.service";
import { Project } from '../../../models/project';
import { ProjectService } from '../../../services/project.service';
import { BsModalRef } from 'ngx-bootstrap/modal/bs-modal-ref.service';
import { BsModalService } from 'ngx-bootstrap/modal';
import { User } from "../../../models/user";
import { UserService } from '../../../services/user.service';

@Component({
  selector: 'app-add-project',
  templateUrl: './add-project.component.html',
  styleUrls: ['./add-project.component.css']
})
export class AddProjectComponent implements OnInit {

  data: Project;
  isEditMode: boolean;
  modalRef: BsModalRef;
  userList: Array<User>;

  @Output()
  loadProjects = new EventEmitter();

  constructor(private loaderService: LoaderService, private projectService: ProjectService, private modalService: BsModalService,
    private userService: UserService) { }

  ngOnInit() {
    this.data = new Project();
  }

  addProject() {
    let newProject: Project;
    console.log(this.data);
    if (!this.data.enableStartAndEndDates) {
      this.data.startDate = null;
      this.data.endDate = null;
    }
    this.loaderService.showLoading(true);
    console.log(this.data);
    this.projectService.addProject(this.data).subscribe((data) => {
      this.loaderService.showSuccess('Project added successfully');
      this.resetProject();
      this.loadProjects.emit(null);
      this.loaderService.showLoading(false);
      newProject = data;
    },
      (error) => {
        this.loaderService.showError(error);
        this.loaderService.showLoading(false);
      });
      return newProject;
  }

  editProject(project: Project) {
    this.data = new Project();
    this.data.name = project.name;
    if (project.startDate != null || project.startDate != undefined) {
      this.data.startDate = project.startDate.split('T')[0];
    }
    if (project.endDate != null || project.endDate != undefined) {
      this.data.endDate = project.endDate.split('T')[0];
    }
    this.data.priority = project.priority;
    if (project.startDate != null && project.endDate != null) {
      this.data.enableStartAndEndDates = true;
    }
    else {
      this.data.enableStartAndEndDates = false;
    }
    this.data.id = project.id;
    console.log("Selected Project")
    console.log(project);
    if(project.user!=undefined)
    {
      this.data.user = new User();
      this.data.user.id=project.user.id;
      this.data.user.firstName=project.user.firstName;
      this.data.user.lastName=project.user.lastName;
      this.data.user.employeeId=project.user.employeeId;
      this.data.managerUserId = project.user.id;
    }
    this.isEditMode = true;
  }

  updateProject() {
    let newProject: Project;
    console.log("Update Project Invoked")
    console.log(this.data);
    this.loaderService.showLoading(true);
    this.projectService.updateProject(this.data).subscribe((data) => {
      this.loaderService.showSuccess('Project updated successfully')
      this.resetProject();
      this.isEditMode = false;
      this.loadProjects.emit(null);
      this.loaderService.showLoading(false);
      newProject=data;
    },
      (error) => {
        this.loaderService.showError(error);
        this.loaderService.showLoading(false);
        this.resetProject();
        this.isEditMode = false;
      });
      return newProject;
  }

  resetProject() {
    this.data = new Project();
  }

  validateProject() {
    if (this.data.name===undefined || this.data.name === null || this.data.name.trim() === '') {
      this.loaderService.showWarning('Please enter project name');
      return false;
    }
    else if (this.data.enableStartAndEndDates && (this.data.startDate === null || this.data.startDate === undefined)) {
      this.loaderService.showWarning('Please enter project start date');
      return false;
    }
    else if (this.data.enableStartAndEndDates && (this.data.endDate === null || this.data.endDate === undefined)) {
      this.loaderService.showWarning('Please enter project end date');
      return false;
    }
    else {
      return true;
    }
  }

  openModal(template: TemplateRef<any>) {
    this.userService.getUser().subscribe((users) => {
      this.userList = users;
      console.log(users);
      this.loaderService.showLoading(false);
    },
      (error) => {
        this.loaderService.showError(error);
        this.loaderService.showLoading(false);
      });
    this.modalRef = this.modalService.show(template);
    document.getElementsByTagName("modal-container")[0].classList.remove("fade");

  }

  selectManager(user: User) {
    if (user != undefined) {
      this.data.user = user;
      this.data.managerUserId=user.id;
      this.modalRef.hide();
    }
  }
}
