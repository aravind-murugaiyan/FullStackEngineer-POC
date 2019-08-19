import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddTaskComponent } from './add-task.component';
import { LoaderService } from "../../../services/shared/loader.service";
import { UserService } from "../../../services/user.service";
import { BaseService } from "../../../services/shared/base.service";
import { ToastrModule } from 'ngx-toastr';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule } from '@angular/common/http';
import { FilteruserPipe } from '../../../pipes/filteruser.pipe';
import { BsDatepickerModule, ModalModule } from 'ngx-bootstrap';
import { FormsModule } from '@angular/forms';
import { Ng5SliderModule } from 'ng5-slider';
import { TaskService } from '../../../services/task.services';
import { ProjectService } from '../../../services/project.service';
import { RouterTestingModule } from '@angular/router/testing';
import { MockTaskService } from '../../../services/test/mock-task.service';
import { MockProjectService } from '../../../services/test/mock-project.service';
import { MockUserService } from '../../../services/test/mock-user.service';
import { Task } from '../../../models/task';
import { Project } from '../../../models/project';

describe('AddTaskComponent', () => {
  let component: AddTaskComponent;
  let fixture: ComponentFixture<AddTaskComponent>;
  let userService: UserService;
  let projectService: ProjectService;
  let taskService: TaskService;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [FormsModule, Ng5SliderModule, HttpClientModule,RouterTestingModule,
        BsDatepickerModule.forRoot(),
        ModalModule.forRoot(),
        ToastrModule.forRoot({
          timeOut: 3000,
          positionClass: 'toast-top-center',
          preventDuplicates: true
        }),
        BrowserAnimationsModule],
      declarations: [AddTaskComponent, FilteruserPipe],
      providers: [LoaderService, BaseService, 
        {provide: TaskService, useClass: MockTaskService}, 
        {provide: ProjectService, useClass: MockProjectService}, 
        {provide: UserService, useClass: MockUserService}]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddTaskComponent);
    component = fixture.componentInstance;
    userService = TestBed.get(UserService);
    projectService = TestBed.get(ProjectService);
    taskService = TestBed.get(TaskService);
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('task with id 3 should get added', () => {
    component.data = new Task();
    component.data.name = "Task 3";
    component.data.isParentTask=true;
    component.data.project = new Project();
    component.data.project.id=1;
    component.data.projectId=1;
    var newTask = component.addTask();
    expect(newTask.id).toBe(3);
  });

  // it('task with id 2 should get updated', () => {
  //   component.data = new Task();
  //   component.data.name = "Task 02";
  //   component.data.id=2;
  //   component.data.project=new Project();
  //   component.data.projectId=1;
  //   var upadtedUser = component.updateTask();
    
  //   expect(upadtedUser.name).toBe("Task 02");
  // });

  it('user details cannot be empty', () => {
    let newTask:Task = new Task();
    var isValidRequest = component.validateTaskData(newTask);    
    expect(isValidRequest).toBe(false);
  });


});
