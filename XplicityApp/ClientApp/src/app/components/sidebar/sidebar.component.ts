import { Component, OnInit } from '@angular/core';
import { first } from 'rxjs/operators';
import { Router } from '@angular/router';
import {FormControl} from '@angular/forms';

import { TableRowUserModel } from '../../models/table-row-user-model';
import { AuthenticationService } from '../../services/authentication.service';
import { UserService } from '../../services/user.service';
import { Holiday } from '../../models/holiday';
import { HolidaysService } from '../../services/holidays.service';

@Component({ templateUrl: 'sidebar.component.html', styleUrls: ['sidebar.component.scss'] })
export class SidebarComponent implements OnInit {

  mode = new FormControl('side');
  currentUser: TableRowUserModel;
  users = [];
  holidays: Holiday[];
  errorMessage: string;
  role: string;

  constructor(
    private authenticationService: AuthenticationService,
    private router: Router,
    private holidaysService: HolidaysService,
    private userService: UserService
  ) {
    this.currentUser = new TableRowUserModel();
  }

  deleteUser(id: number) {
    this.userService.deleteUser(id).pipe(first()).subscribe(() => this.loadAllUsers());
  }

  private loadAllUsers() {
    this.userService.getAllUsers().pipe(first()).subscribe(users => this.users = users);
  }

  logout() {
    this.authenticationService.logout();
    this.router.navigate(['/login']);
  }

  ngOnInit() {
    this.holidaysService.getHolidays().subscribe(holidays => {
      this.holidays = holidays;
    }, error => {
      console.error(error);
      this.errorMessage = error.message;
    });

    this.userService.getCurrentUser().subscribe(user => {
      this.currentUser = user;
    });
    this.role = this.userService.getRole();
  }

  isAdmin(): boolean {
    return this.userService.isAdmin();
  }
}