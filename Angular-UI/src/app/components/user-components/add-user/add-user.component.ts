import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { User } from '../../../models/user';
import { LoaderService } from "../../../services/shared/loader.service";
import { UserService } from "../../../services/user.service";
import { SearchUsersComponent } from "../search-users/search-users.component"

@Component({
  selector: 'app-add-user',
  templateUrl: './add-user.component.html',
  styleUrls: ['./add-user.component.css']
})
export class AddUserComponent implements OnInit {

  data: User;
  isEditMode: boolean;

  @Output()
  loadUsers = new EventEmitter();

  constructor(private userService: UserService, private loaderService: LoaderService) {

  }

  ngOnInit() {
    this.data = new User();
    this.isEditMode = false;
  }

  addUser() {
    let newUser: User;
    if (this.validateUserData()) {
      this.loaderService.showLoading(true);
      this.userService.addUser(this.data).subscribe((data) => {
        this.loaderService.showSuccess('Saved successfully')
        this.resetControls();
        this.loaderService.showLoading(false);
        this.loadUsers.emit(null);
        newUser = data;
      },
        (error) => {
          this.loaderService.showError(error);
          this.loaderService.showLoading(false);
        });
    }
    return newUser;
  };

  resetControls() {
    this.data = new User();
    this.isEditMode = false;
  }

  validateUserData() {
    if (this.data.firstName === undefined || this.data.firstName === '') {
      console.log('Please add first Name');
      this.loaderService.showWarning('Please add first Name ');
      return false;
    }
    else if (this.data.lastName === undefined || this.data.lastName === '') {
      console.log('Please add last Name');
      this.loaderService.showWarning('Please add last Name ');
      return false;
    }
    else if (this.data.employeeId === undefined || this.data.employeeId === null) {
      console.log('Please add empId');
      this.loaderService.showWarning('Please enter employee id');
      return false;
    }
    else {
      return true;
    }
  }

  editUser(userData: User) {
    this.data.firstName = userData.firstName;
    this.data.lastName = userData.lastName;
    this.data.employeeId = userData.employeeId;
    this.data.id = userData.id;
    this.isEditMode = true;
  }

  updateUser() {
    let updatedUser: User;
    if (this.validateUserData()) {
      this.loaderService.showLoading(true);
      this.userService.updateUser(this.data).subscribe((data) => {
        this.loaderService.showSuccess('Update successfully')
        this.resetControls();
        this.loadUsers.emit(null);
        this.loaderService.showLoading(false);
        updatedUser = data;
      },
        (error) => {
          this.loaderService.showError(error);
          this.loaderService.showLoading(false);
        });
    }
    return updatedUser;
  }
}
