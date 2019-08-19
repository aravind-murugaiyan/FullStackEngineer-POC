import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { AddTaskComponent } from './components/task-components/add-task/add-task.component';
import { ViewTaskComponent } from './components/task-components/view-task/view-task.component';
import { ManageUserComponent } from './components/user-components/manage-user/manage-user.component';
import { ManageProjectComponent } from './components/project-components/manage-project/manage-project.component';

const routes: Routes = [
  { path: '', redirectTo: 'manage-user', pathMatch: 'full' },
  { path: 'manage-user', component: ManageUserComponent },
  { path: 'manage-project', component: ManageProjectComponent },
  { path: 'add-task', component: AddTaskComponent },
  { path: 'view-task', component: ViewTaskComponent },
  { path: '**', redirectTo: 'manage-user' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
