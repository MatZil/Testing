import { AuthenticationService } from '../services/authentication-service.service';
import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { HomeComponent } from '../components/home/home.component';
import decode from 'jwt-decode';

@Injectable()
export class RoleGuard implements CanActivate {
  constructor(private authService: AuthenticationService, private router: Router, private home: HomeComponent) {
  }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {

    //const expectedRole = route.data.expectedRole;

    //const token = localStorage.getItem('jwtkey');
    //console.log(token);
    // const tokenPayload = decode(token);

    //if (this.authService.currentUserValue === null || tokenPayload.role !== expectedRole) {
    //   this.router.navigate(['home']);
    //  return false;
    // }
    return true;
  }
}
