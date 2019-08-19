import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewTaskComponent } from './view-task.component';
import { FormsModule } from '@angular/forms';
import { LoaderService } from "../../../services/shared/loader.service";
import { BaseService } from "../../../services/shared/base.service";
import { ToastrModule } from 'ngx-toastr';
import { HttpClientModule } from '@angular/common/http';
import { FilteruserPipe } from '../../../pipes/filteruser.pipe';
import { BsDatepickerModule, ModalModule } from 'ngx-bootstrap';
import { TaskService } from '../../../services/task.services';
import { ProjectService } from '../../../services/project.service';
import { RouterTestingModule } from '@angular/router/testing';
import { Project } from '../../../models/project';
import { UserService } from '../../../services/user.service';
import { MockTaskService } from '../../../services/test/mock-task.service';
import { MockProjectService } from '../../../services/test/mock-project.service';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { Task } from '../../../models/task';


describe('ViewTaskComponent', () => {
  let component: ViewTaskComponent;
  let fixture: ComponentFixture<ViewTaskComponent>;
  let taskService: TaskService;
  let projectService: ProjectService;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [FormsModule, HttpClientModule, RouterTestingModule,BrowserAnimationsModule,
        BsDatepickerModule.forRoot(),
        ModalModule.forRoot(),
        ToastrModule.forRoot({
          timeOut: 3000,
          positionClass: 'toast-top-center',
          preventDuplicates: true

        })],
      declarations: [ViewTaskComponent, FilteruserPipe],
      providers: [LoaderService, BaseService,
        { provide: TaskService, useClass: MockTaskService },
        { provide: ProjectService, useClass: MockProjectService }]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ViewTaskComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('system should have 2 tasks', () => {
    let project: Project = new Project();
    project.id = 1;
    component.loadTaksByProject(project);
    expect(component.taskList.length).toBe(2);
  });

  it('task with id 1 should get completed', () => {
    let task: Task = new Task();
    task.id=1;
    var isDeleted = component.deleteTask(task);
    expect(isDeleted).toBe(true);
  });

});
