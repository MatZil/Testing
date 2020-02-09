import { Component, OnInit } from '@angular/core';
import { TableRowUserModel } from '../../models/table-row-user-model';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-overtime-display',
  templateUrl: './overtime-display.component.html',
  styleUrls: ['./overtime-display.component.scss']
})
export class OvertimeDisplayComponent implements OnInit {
  currentUser: TableRowUserModel;

  constructor(
    private userService: UserService) {}

  ngOnInit() {
    this.userService.getCurrentUser().subscribe(user => {
      this.currentUser = user;
    });
  }

}
