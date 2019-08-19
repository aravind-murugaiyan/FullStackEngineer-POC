import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddProjectComponent } from './add-project.component';
import { FormsModule } from '@angular/forms';
import { Ng5SliderModule } from 'ng5-slider';

import { LoaderService } from "../../../services/shared/loader.service";
import { BaseService } from "../../../services/shared/base.service";
import { ToastrModule } from 'ngx-toastr';
import { HttpClientModule } from '@angular/common/http';
import { FilteruserPipe } from '../../../pipes/filteruser.pipe';
import { BsDatepickerModule, ModalModule } from 'ngx-bootstrap';
import { ProjectService } from '../../../services/project.service';
import { UserService } from '../../../services/user.service';
import { MockProjectService } from '../../../services/test/mock-project.service';
import { MockUserService } from '../../../services/test/mock-user.service';
import { Project } from '../../../models/project';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

describe('AddProjectComponent', () => {
  let component: AddProjectComponent;
  let fixture: ComponentFixture<AddProjectComponent>;
  let projectService: ProjectService;
  let userService: UserService;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [FormsModule, Ng5SliderModule, HttpClientModule,BrowserAnimationsModule,
        BsDatepickerModule.forRoot(),
        ModalModule.forRoot(),
        ToastrModule.forRoot({
          timeOut: 3000,
          positionClass: 'toast-top-center',
          preventDuplicates: true

        })],
      declarations: [AddProjectComponent, FilteruserPipe],
      providers: [LoaderService, BaseService,
         {provide: ProjectService, useClass:MockProjectService },
         { provide: UserService, useClass: MockUserService}]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddProjectComponent);
    component = fixture.componentInstance;
    projectService = TestBed.get(ProjectService);
    userService = TestBed.get(UserService);
    projectService = TestBed.get(ProjectService);
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('project with id 1 should get added', () => {
    component.data = new Project();
    component.data.name = "Project 3"; 
    var newProject = component.addProject();
    expect(newProject.id).toBe(3);
  });

  it('project with id 2 should get updated', () => {
    component.data = new Project();
    component.data.name = "Proejct 02";
    component.data.id=2;
    var upadtedUser = component.updateProject();    
    expect(upadtedUser.name).toBe("Proejct 02");
  });

  it('project details cannot be empty', () => {
    component.data = new Project();
    var isValidRequest = component.validateProject();    
    expect(isValidRequest).toBe(false);
  });

});
