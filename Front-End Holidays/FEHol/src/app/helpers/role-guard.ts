import { AuthenticationService } from '../services/authentication-service.service';
import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { SidebarComponent } from '../components/sidebar/sidebar.component';

@Injectable()
export class RoleGuard implements CanActivate {
  constructor(private authService: AuthenticationService, private router: Router, private sideBar: SidebarComponent) {
  }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {

    if (this.sideBar.isAdmin()) {
      return true;
    }

    // navigate to not found page
    this.router.navigate(['/home'], { queryParams: { returnUrl: state.url } });
    return false;
  }
}
