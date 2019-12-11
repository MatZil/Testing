import { Component, OnInit } from '@angular/core';
import { User } from '../../models/user';
import { Requestholidays } from '../../models/requestholidays';
import { AuthenticationService } from '../../services/authentication.service';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-overtime-display',
  templateUrl: './overtime-display.component.html',
  styleUrls: ['./overtime-display.component.scss']
})
export class OvertimeDisplayComponent implements OnInit {
  currentUser: User;
  currentUserId: number;
  requestHolidays: Requestholidays = new Requestholidays();

  constructor(
    private authenticationService: AuthenticationService,
    private userService: UserService
  ) {
    this.currentUserId = this.authenticationService.getUserId();
    this.requestHolidays.employeeId = this.currentUserId;
  }

  ngOnInit() {
    this.userService.getCurrentUser().subscribe(user => {
      this.currentUser = user;
    });
  }

}
