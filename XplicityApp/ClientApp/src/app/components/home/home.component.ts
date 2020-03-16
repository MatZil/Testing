import { Component, OnInit } from '@angular/core';
import { TableRowUserModel } from '../../models/table-row-user-model';
import { UserService } from '../../services/user.service';
import { MatDialog } from '@angular/material';
import { HolidayRequestFormComponent } from '../holiday-request-form/holiday-request-form.component';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  currentUser: TableRowUserModel;
  image: string;

  constructor(private userService: UserService) {
    this.image = 'assets/bg.jpg';
  }

  ngOnInit() {
    this.userService.getCurrentUser().subscribe(user => {
      this.currentUser = user;
    });
  }
}
