import { Component } from '@angular/core';
import { Router } from '@angular/router';

import { AuthenticationService } from './services/authentication.service';
import { TableRowUserModel } from './models/table-row-user-model';

@Component({ selector: 'app-root', templateUrl: './app.component.html' })
export class AppComponent {
  title = 'FEHol';
  isCollapsed = false;
  currentUser: TableRowUserModel;

  constructor(
    private router: Router,
    private authenticationService: AuthenticationService
  ) {
    // this.authenticationService.currentUser.subscribe(x => this.currentUser = x);
  }

  logout() {
    this.authenticationService.logout();
    this.router.navigate(['/login']);
  }
}
