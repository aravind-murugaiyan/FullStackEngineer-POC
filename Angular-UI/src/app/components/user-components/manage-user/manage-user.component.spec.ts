import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { ManageUserComponent } from './manage-user.component';
import { AddUserComponent } from '../add-user/add-user.component';
import { SearchUsersComponent } from '../search-users/search-users.component';
import { FormsModule } from '@angular/forms';
import { FilteruserPipe } from '../../../pipes/filteruser.pipe';
import { LoaderService } from '../../../services/shared/loader.service';
import { BaseService } from '../../../services/shared/base.service';
import { UserService } from '../../../services/user.service';
import { HttpClientModule } from '@angular/common/http';
import { ToastrModule } from 'ngx-toastr';
import { MockUserService } from '../../../services/test/mock-user.service';

describe('ManageUserComponent', () => {
  let component: ManageUserComponent;
  let fixture: ComponentFixture<ManageUserComponent>;
  let userService: UserService;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [FormsModule, HttpClientModule,
        ToastrModule.forRoot({
          timeOut: 3000,
          positionClass: 'toast-top-center',
          preventDuplicates: true
          
        })],
      declarations: [ManageUserComponent, AddUserComponent, SearchUsersComponent, FilteruserPipe],
      providers: [LoaderService, BaseService,
      {provide: UserService, useClass: MockUserService}]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ManageUserComponent);
    component = fixture.componentInstance;
    userService = TestBed.get(UserService);
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
