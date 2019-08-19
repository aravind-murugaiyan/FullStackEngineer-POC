import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Ng5SliderModule } from 'ng5-slider';

import { LoaderService } from "./services/shared/loader.service";
import { UserService } from "./services/user.service";
import { BaseService } from "./services/shared/base.service";
import { ToastrModule } from 'ngx-toastr';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule } from '@angular/common/http';
import { FilteruserPipe } from './pipes/filteruser.pipe';
import { BsDatepickerModule, ModalModule } from 'ngx-bootstrap';


import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AddTaskComponent } from './components/task-components/add-task/add-task.component';
import { ViewTaskComponent } from './components/task-components/view-task/view-task.component';
import { AddUserComponent } from './components/user-components/add-user/add-user.component';
import { AddProjectComponent } from './components/project-components/add-project/add-project.component';
import { SearchUsersComponent } from './components/user-components/search-users/search-users.component';
import { ManageUserComponent } from './components/user-components/manage-user/manage-user.component';
import { ManageProjectComponent } from './components/project-components/manage-project/manage-project.component';
import { SearchProjectsComponent } from './components/project-components/search-projects/search-projects.component';
import { ProjectService } from './services/project.service';
import { TaskService } from './services/task.services';

@NgModule({
  declarations: [
    AppComponent,
    AddTaskComponent,
    ViewTaskComponent,
    AddUserComponent,
    AddProjectComponent,
    SearchUsersComponent,
    ManageUserComponent,
    ManageProjectComponent,
    SearchProjectsComponent,
    FilteruserPipe,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    BrowserAnimationsModule,
    BsDatepickerModule.forRoot(),
    HttpClientModule,
    ModalModule.forRoot(),
    ToastrModule.forRoot({
      timeOut: 3000,
      positionClass: 'toast-top-center',
      preventDuplicates: true
      
    }),
    Ng5SliderModule
  ],
  providers: [LoaderService, BaseService, UserService, SearchUsersComponent, ProjectService, TaskService],
  bootstrap: [AppComponent]
})
export class AppModule { }
