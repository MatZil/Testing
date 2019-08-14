import { AuthenticationService } from '../services/authentication-service.service';
import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { HomeComponent } from '../components/home/home.component';

@Injectable()
export class RoleGuard implements CanActivate {
  constructor(private authService: AuthenticationService, private router: Router, private home: HomeComponent) {
  }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {

    if (this.home.isAdmin()) {
      return true;
    }

    // navigate to not found page
    this.router.navigate(['/home'], { queryParams: { returnUrl: state.url } });
    return false;
  }
}
