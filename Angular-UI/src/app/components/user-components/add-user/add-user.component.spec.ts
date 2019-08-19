import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { LoaderService } from "../../../services/shared/loader.service";
import { UserService } from "../../../services/user.service";
import { BaseService } from "../../../services/shared/base.service";
import { ToastrModule } from 'ngx-toastr';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { AddUserComponent } from './add-user.component';
import { MockUserService } from '../../../services/test/mock-user.service';
import { User } from '../../../models/user';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

describe('AddUserComponent', () => {
  let component: AddUserComponent;
  let fixture: ComponentFixture<AddUserComponent>;
  let userService: UserService;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [FormsModule, HttpClientModule,BrowserAnimationsModule,
        ToastrModule.forRoot({
          timeOut: 3000,
          positionClass: 'toast-top-center',
          preventDuplicates: true
          
        })],
      declarations: [ AddUserComponent ],
      providers: [LoaderService, BaseService, {provide: UserService, useClass: MockUserService}]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddUserComponent);
    component = fixture.componentInstance;
    userService = TestBed.get(UserService);
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('user with id 3 should get added', () => {
    component.data = new User();
    component.data.firstName = "Aravind";
    component.data.lastName = "Murugaiyan";
    component.data.employeeId = "267127";
    var newUser = component.addUser();
    expect(newUser.id).toBe(2);
  });
  
  it('user details cannot be empty', () => {
    component.data = new User();
    var isValidRequest = component.validateUserData();    
    expect(isValidRequest).toBe(false);
  });

});
