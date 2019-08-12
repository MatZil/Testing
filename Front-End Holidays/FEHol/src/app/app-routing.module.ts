import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { CalendarComponent } from './components/calendar/calendar.component';
import { LoginComponent } from './components/login/login.component';
import { ClientTableComponent } from './components/client-table/client-table.component';
import { ErrorPageComponent } from './components/error-page/error-page.component';
import { EmployeesTableComponent } from './components/employees-table/employees-table.component';
import { PolicyComponent } from './components/policy/policy.component';
import { AuthGuard } from './helpers/auth-guard';
import { RoleGuard } from './helpers/role-guard';


const routes: Routes = [
  {path: 'home', component: HomeComponent, canActivate: [AuthGuard],
    children: [
      {path: '', component: PolicyComponent, canActivate: [AuthGuard]}
    ]},
  {path: 'login', component: LoginComponent},
  {path: 'calendar', component: HomeComponent, canActivate: [AuthGuard],
   children: [
     {path: '', component: CalendarComponent, canActivate: [AuthGuard]}
      ]},
  {path: 'clients', component: HomeComponent, canActivate: [AuthGuard],
   children: [
     {path: '', component: ClientTableComponent, canActivate: [AuthGuard]}
      ]},
  {path: 'employees', component: HomeComponent, canActivate: [AuthGuard],
   children: [
     {path: '', component: EmployeesTableComponent, canActivate: [AuthGuard]}
   ]},
  {path: '', redirectTo: 'home', pathMatch: 'full'},
  {path: '**', component: ErrorPageComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
