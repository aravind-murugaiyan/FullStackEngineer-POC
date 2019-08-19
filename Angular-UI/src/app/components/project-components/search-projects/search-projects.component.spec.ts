import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SearchProjectsComponent } from './search-projects.component';
import { FormsModule } from '@angular/forms';
import { LoaderService } from "../../../services/shared/loader.service";
import { BaseService } from "../../../services/shared/base.service";
import { ToastrModule } from 'ngx-toastr';
import { HttpClientModule } from '@angular/common/http';
import { ProjectService } from '../../../services/project.service';
import { MockProjectService } from '../../../services/test/mock-project.service';
import { Project } from '../../../models/project';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

describe('SearchProjectsComponent', () => {
  let component: SearchProjectsComponent;
  let fixture: ComponentFixture<SearchProjectsComponent>;
  let projectService: ProjectService;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports:[FormsModule, HttpClientModule,BrowserAnimationsModule,
        ToastrModule.forRoot({
          timeOut: 3000,
          positionClass: 'toast-top-center',
          preventDuplicates: true
          
        })],
      declarations: [SearchProjectsComponent],
      providers: [LoaderService, BaseService, {provide: ProjectService, useClass: MockProjectService}]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SearchProjectsComponent);
    component = fixture.componentInstance;
    projectService = TestBed.get(ProjectService);
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('system should have 2 projects', () => {
    expect(component.projectList.length).toBe(2);
  });

  it('project with id 1 should get deleted', () => {
    var proj = new Project();
    proj.id=2;
    var isDeleted = component.deleteProject(proj);
    expect(isDeleted).toBe(true);
  });
});
