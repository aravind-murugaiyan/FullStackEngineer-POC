import { Component, OnInit, ViewChild } from '@angular/core';
import { SearchProjectsComponent } from '../search-projects/search-projects.component';
import { AddProjectComponent } from '../add-project/add-project.component';
import { Project } from '../../../models/project';

@Component({
  selector: 'app-manage-project',
  templateUrl: './manage-project.component.html',
  styleUrls: ['./manage-project.component.css']
})
export class ManageProjectComponent implements OnInit {
   
  @ViewChild(SearchProjectsComponent, ({ static: false })) childSearch: SearchProjectsComponent;
  @ViewChild(AddProjectComponent, ({ static: false })) childProject: AddProjectComponent;

  constructor() { }

  ngOnInit() {
  }

  editProject(project: Project)
  {
    this.childProject.editProject(project);
  }

  loadProjectList()
  {
    this.childSearch.loadProjectList();
  }

}
