import { Component, OnInit } from '@angular/core';
import { User } from '../../models/user';
import { AuthenticationService } from '../../services/authentication-service.service';
import { UserService } from '../../services/user.service';
import { PolicyService } from '../../services/policy.service';


@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {

  currentUser: User;
  holidaysCount: number;

  constructor(
    private authenticationService: AuthenticationService,
    private userService: UserService,
    private policyService: PolicyService
  ) {
    this.currentUser = this.authenticationService.currentUserValue;
  }

  ngOnInit() {
    this.userService.getUser(this.authenticationService.getUserId()).subscribe(user => {
      this.currentUser = user;
      this.getCount(this.currentUser.id);
    });
  }

  getCount(userId: number) {
    this.policyService.getHolidaysCount(userId).subscribe(count => {
      this.holidaysCount = count;
    });
  }

}
