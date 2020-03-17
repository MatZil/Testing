import { Component, OnInit } from '@angular/core';
import { TableRowUserModel } from '../../models/table-row-user-model';
import { UserService } from '../../services/user.service';
import { MatDialog } from '@angular/material/dialog';
import { EnumToStringConverterService } from 'src/app/services/enum-to-string-converter.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  currentUser: TableRowUserModel;
  image: string;

  constructor(
    private userService: UserService,
    public enumConverter: EnumToStringConverterService,
    public dialog: MatDialog
  ) {
    this.image = 'assets/bg.jpg';
  }

  ngOnInit() {
    this.userService.getCurrentUser().subscribe(user => {
      this.currentUser = user;
    });
  }


}
