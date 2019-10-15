import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { CalendarComponent } from './components/calendar/calendar.component';
import { LoginComponent } from './components/login/login.component';
import { ClientTableComponent } from './components/client-table/client-table.component';
import { ErrorPageComponent } from './components/error-page/error-page.component';
import { EmployeesTableComponent } from './components/employees-table/employees-table.component';
import { HolidaysTableComponent } from './components/holidays-table/holidays-table.component';
import { PolicyComponent } from './components/policy/policy.component';
import { AuthGuard } from './helpers/auth-guard';
import { RoleGuardService } from './helpers/role-guard';
import { ProfileComponent } from './components/profile/profile.component';


const routes: Routes = [
  {
    path: 'home', component: HomeComponent, canActivate: [AuthGuard],
    children: [
      { path: '', component: PolicyComponent, canActivate: [AuthGuard] }
    ]
  },
  { path: 'login', component: LoginComponent },
  {
    path: 'calendar', component: HomeComponent, canActivate: [AuthGuard], data: {
      expectedRole: 'Employee'
    },
    children: [
      {
        path: '', component: CalendarComponent, canActivate: [AuthGuard]
      }
    ]
  },
  {
    path: 'clients', component: HomeComponent, canActivate: [RoleGuardService], data: {
      expectedRole: 'admin'
    },
    children: [
      {
        path: '', component: ClientTableComponent, canActivate: [RoleGuardService], data: {
          expectedRole: 'admin'
        }
      }
    ]
  },
  {
    path: 'employees', component: HomeComponent, canActivate: [RoleGuardService], data: {
      expectedRole: 'admin'
    },
    children: [
      {
        path: '', component: EmployeesTableComponent, canActivate: [RoleGuardService], data: {
          expectedRole: 'admin'
        }
      }
    ]
  },
  {
    path: 'holidays', component: HomeComponent, canActivate: [AuthGuard],
    children: [
      { path: '', component: HolidaysTableComponent, canActivate: [AuthGuard] }
    ]
  },
  {
    path: 'profile', component: HomeComponent, canActivate: [AuthGuard],
    children: [
      { path: '', component: ProfileComponent, canActivate: [AuthGuard] }
    ]
  },
  { path: '', redirectTo: 'home', pathMatch: 'full' },
  { path: '**', component: ErrorPageComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
