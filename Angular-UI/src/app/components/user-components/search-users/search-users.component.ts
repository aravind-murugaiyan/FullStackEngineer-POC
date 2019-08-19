import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { User } from "../../../models/user";
import { UserService } from "../../../services/user.service";
import { LoaderService } from "../../../services/shared/loader.service";

@Component({
  selector: 'app-search-users',
  templateUrl: './search-users.component.html',
  styleUrls: ['./search-users.component.css']
})
export class SearchUsersComponent implements OnInit {

  userList: Array<User>;
  searchText: string;
  sortAscFirstName: boolean;
  sortAscLastName: boolean;
  sortAscEmpId: boolean;

  @Output()
  editSelectedUser = new EventEmitter<User>();

  constructor(private userService: UserService, private loaderService: LoaderService) { }

  ngOnInit() {
    this.loadUsers();
  }

  loadUsers() {
    this.loaderService.showLoading(true);
    this.userService.getUser().subscribe((users) => {
      this.userList = users;
      console.log(users);
      this.loaderService.showLoading(false);
    },
      (error) => {
        this.loaderService.showError(error);
        this.loaderService.showLoading(false);
      });
  }

  deleteUser(user) {
    var isDeleted = false;
    this.loaderService.showLoading(true);
    this.userService.deleteUser(user).subscribe((data) => {
      this.loaderService.showSuccess('User Deleted successfully')
      this.ngOnInit();
      this.loaderService.showLoading(false);
      isDeleted=true;
    },
      (error) => {
        this.loaderService.showError(error);
        this.loaderService.showLoading(false);
      });
      return isDeleted;
  }

  editUser(user) {
    console.log("edit user called");
    this.editSelectedUser.emit(user);
  }

  sortUserByFirstName() {
    if (this.sortAscFirstName === undefined || this.sortAscFirstName === false) {
      this.sortAscFirstName = true;
    }
    else {
      this.sortAscFirstName = false;
    }
    this.sortAscLastName = undefined;
    this.sortAscEmpId = undefined;
    this.loadSortedUsers({ propertyName: 'firstName', sortAsc: this.sortAscFirstName });
  }

  loadSortedUsers({ propertyName, sortAsc }: { propertyName: string; sortAsc: boolean; }) {

    this.loaderService.showLoading(true);
    this.userService.getSortedUser({ propertyName, sortAsc }).subscribe((users) => {
      this.userList = users;
      this.loaderService.showLoading(false);
    },
      (error) => {
        this.loaderService.showError(error);
        this.loaderService.showLoading(false);
      });
  }


  sortUserByLastName() {
    if (this.sortAscLastName === undefined || this.sortAscLastName === false) {
      this.sortAscLastName = true;
    }
    else {
      this.sortAscLastName = false;
    }
    this.sortAscFirstName = undefined;
    this.sortAscEmpId = undefined;
    console.log("sort order"+this.sortAscLastName);
    this.loadSortedUsers({ propertyName: 'lastName', sortAsc: this.sortAscLastName });
  }

  sortUserByEmpId() {
    if (this.sortAscEmpId === undefined || this.sortAscEmpId === false) {
      this.sortAscEmpId = true;
    }
    else {
      this.sortAscEmpId = false;
    }
    this.sortAscFirstName = undefined;
    this.sortAscLastName = undefined;
    this.loadSortedUsers({ propertyName: 'empId', sortAsc: this.sortAscEmpId });
  }
}
