import { Component, OnInit, ViewChild } from '@angular/core';
import { User } from "../../../models/user";
import { SearchUsersComponent } from "../search-users/search-users.component";
import { AddUserComponent } from "../add-user/add-user.component";

@Component({
  selector: 'app-manage-user',
  templateUrl: './manage-user.component.html',
  styleUrls: ['./manage-user.component.css']
})
export class ManageUserComponent implements OnInit {

  @ViewChild(SearchUsersComponent, ({ static: false })) childSearch: SearchUsersComponent;
  @ViewChild(AddUserComponent, ({ static: false })) childUser: SearchUsersComponent;

  constructor() { }

  ngOnInit() {
  }

  loadUsers()
  {  
    this.childSearch.loadUsers();
  }

  editUser(userData: User)
  {
    this.childUser.editUser(userData);
  }
}
