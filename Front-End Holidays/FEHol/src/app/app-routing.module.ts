import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { CalendarComponent } from './components/calendar/calendar.component';
import { AuthGuard } from './helpers/auth-guard';
import { LoginComponent } from './components/login/login.component';
import { ClientTableComponent } from './components/client-table/client-table.component';


const routes: Routes = [
  {path: 'home', component: HomeComponent, canActivate: [AuthGuard]},
  {path: 'login', component: LoginComponent},
  {path: 'calendar', component: HomeComponent, canActivate: [AuthGuard],
   children: [
     {path: '', component: CalendarComponent, canActivate: [AuthGuard]}
      ]},
  {path: 'clients', component: HomeComponent, canActivate: [AuthGuard],
   children: [
     {path: '', component: ClientTableComponent, canActivate: [AuthGuard]}
      ]},
  {path: '**', component: HomeComponent, canActivate: [AuthGuard]}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
