import { Component, OnInit } from '@angular/core';
import { first } from 'rxjs/operators';
import { Router } from '@angular/router';

import { User } from '../../models/user';
import { AuthenticationService } from '../../services/authentication.service';
import { UserService } from '../../services/user.service';
import { Holidays } from '../../models/holidays';
import { HolidaysService } from '../../services/holidays.service';

@Component({ templateUrl: 'sidebar.component.html', styleUrls: ['sidebar.component.scss'] })
export class SidebarComponent implements OnInit {
  isCollapsed = false;
  currentUser: User;
  users = [];
  holidays: Holidays[];
  errorMessage: string;
  role: string;
  constructor(
    private authenticationService: AuthenticationService,
    private router: Router,
    private holidaysService: HolidaysService,
    private userService: UserService
  ) {
    this.currentUser = new User();
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

  isAdmin() {
    if (this.role === 'Admin') {
      return true;
    }
    else {
      return false;
    }
  }
}
