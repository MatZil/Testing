import { Component, OnInit } from '@angular/core';
import { first } from 'rxjs/operators';
import { Router } from '@angular/router';

import { User } from '../../models/user';
import { AuthenticationService } from '../../services/authentication-service.service';
import { UserService } from '../../services/user.service';
import { Holidays } from '../../models/holidays';
import { HolidaysService } from '../../services/holidays.service';

@Component({ templateUrl: 'home.component.html', styleUrls: ['home.component.scss'] })
export class HomeComponent implements OnInit {
  isCollapsed = false;
  currentUser: User;
  users = [];
  holidays: Holidays[];
  errorMessage: string;

  constructor(
    private authenticationService: AuthenticationService,
    private router: Router,
    private holidaysService: HolidaysService,
    private userService: UserService
  ) {
    this.currentUser = this.authenticationService.currentUserValue;
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

    this.userService.getUser(this.authenticationService.getUserId()).subscribe(user => {
      this.currentUser = user;
    });
  }

  isAdmin() {
    if (this.currentUser.role === 'admin') {
      return true;
    }
  }
}
