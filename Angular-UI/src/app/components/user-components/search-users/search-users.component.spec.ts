import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { LoaderService } from "../../../services/shared/loader.service";
import { UserService } from "../../../services/user.service";
import { BaseService } from "../../../services/shared/base.service";
import { ToastrModule } from 'ngx-toastr';
import { HttpClientModule } from '@angular/common/http';
import { FilteruserPipe } from '../../../pipes/filteruser.pipe';

import { SearchUsersComponent } from './search-users.component';
import { FormsModule } from '@angular/forms';
import { MockUserService } from '../../../services/test/mock-user.service';
import { User } from '../../../models/user';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

describe('SearchUsersComponent', () => {
  let component: SearchUsersComponent;
  let fixture: ComponentFixture<SearchUsersComponent>;
  let userService: UserService;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [FormsModule, HttpClientModule,BrowserAnimationsModule,
        ToastrModule.forRoot({
          timeOut: 3000,
          positionClass: 'toast-top-center',
          preventDuplicates: true
        })],
      declarations: [SearchUsersComponent, FilteruserPipe],
      providers: [LoaderService, BaseService, { provide: UserService, useClass: MockUserService }]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SearchUsersComponent);
    component = fixture.componentInstance;
    userService = TestBed.get(UserService);
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('system should have 2 users', () => {
    expect(component.userList.length).toBe(1);
  });

  it('user with id 1 should get deleted', () => {
    var user = new User();
    user.id=2;
    var isDeleted = component.deleteUser(user);
    expect(isDeleted).toBe(true);
  });

});
