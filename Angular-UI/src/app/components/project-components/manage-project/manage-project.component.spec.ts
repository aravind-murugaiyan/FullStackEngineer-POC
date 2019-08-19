import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageProjectComponent } from './manage-project.component';
import { FormsModule } from '@angular/forms';
import { Ng5SliderModule } from 'ng5-slider';
import { LoaderService } from "../../../services/shared/loader.service";
import { UserService } from "../../../services/user.service";
import { BaseService } from "../../../services/shared/base.service";
import { ToastrModule } from 'ngx-toastr';
import { HttpClientModule } from '@angular/common/http';
import { FilteruserPipe } from '../../../pipes/filteruser.pipe';
import { BsDatepickerModule, ModalModule } from 'ngx-bootstrap';
import { AddProjectComponent } from '../add-project/add-project.component';
import { SearchProjectsComponent } from '../search-projects/search-projects.component';
import { ProjectService } from '../../../services/project.service';
import { MockProjectService } from '../../../services/test/mock-project.service';
import { MockUserService } from '../../../services/test/mock-user.service';

describe('ManageProjectComponent', () => {
  let component: ManageProjectComponent;
  let fixture: ComponentFixture<ManageProjectComponent>;
  let projectService: ProjectService;
  let userService: UserService;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [FormsModule, Ng5SliderModule, HttpClientModule,
        BsDatepickerModule.forRoot(),
        ModalModule.forRoot(),
        ToastrModule.forRoot({
          timeOut: 3000,
          positionClass: 'toast-top-center',
          preventDuplicates: true

        })],
      declarations: [ManageProjectComponent, AddProjectComponent, SearchProjectsComponent, FilteruserPipe],
      providers: [LoaderService, BaseService, 
        {provide:ProjectService, useClass:MockProjectService }, 
      {provide:UserService, useClass:MockUserService}]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ManageProjectComponent);
    component = fixture.componentInstance;
    projectService = TestBed.get(ProjectService),
    userService = TestBed.get(UserService);
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
