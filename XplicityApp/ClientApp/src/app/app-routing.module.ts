import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SidebarComponent } from './components/sidebar/sidebar.component';
import { CalendarComponent } from './components/calendar/calendar.component';
import { LoginComponent } from './components/login/login.component';
import { ClientTableComponent } from './components/client-table/client-table.component';
import { ErrorPageComponent } from './components/error-page/error-page.component';
import { EmployeesTableComponent } from './components/employees-table/employees-table.component';
import { HolidaysTableComponent } from './components/holidays-table/holidays-table.component';
import { EmailtemplatesTableComponent } from './components/email-templates-table/email-templates-table.component';
import { HomeComponent } from './components/home/home.component';
import { AuthGuard } from './helpers/auth-guard';
import { RoleGuardService } from './helpers/role-guard';
import { ProfileComponent } from './components/profile/profile.component';
import { InventoryTableComponent } from './components/inventory-table/inventory-table.component';
import { SurveysTableComponent } from './components/surveys-table/surveys-table.component';
import { SurveysAnswersFormComponent } from './components/surveys-answers-form/surveys-answers-form.component';


const routes: Routes = [
  {
    path: 'home', component: SidebarComponent, canActivate: [AuthGuard],
    children: [
      { path: '', component: HomeComponent, canActivate: [AuthGuard] }
    ]
  },
  { path: 'login', component: LoginComponent },
  {
    path: 'calendar', component: SidebarComponent, canActivate: [AuthGuard],
    children: [
      {
        path: '', component: CalendarComponent, canActivate: [AuthGuard]
      }
    ]
  },
  {
    path: 'clients', component: SidebarComponent, canActivate: [AuthGuard],
    children: [
      {
        path: '', component: ClientTableComponent, canActivate: [RoleGuardService], data: {
          expectedRole: 'Admin'
        }
      }
    ]
  },
  {
    path: 'employees', component: SidebarComponent, canActivate: [AuthGuard],
    children: [
      {
        path: '', component: EmployeesTableComponent, canActivate: [RoleGuardService], data: {
          expectedRole: 'Admin'
        }
      }
    ]
  },
  {
    path: 'holidays', component: SidebarComponent, canActivate: [AuthGuard],
    children: [
      { path: '', component: HolidaysTableComponent, canActivate: [AuthGuard] }
    ]
  },
  {
    path: 'inventory-items', component: SidebarComponent, canActivate: [AuthGuard],
    children: [
      {
        path: '', component: InventoryTableComponent, canActivate: [RoleGuardService], data: {
          expectedRole: 'Admin'
        }
      }
    ]
  },
  {
    path: 'emailtemplates', component: SidebarComponent, canActivate: [AuthGuard],
    children: [
      {
        path: '', component: EmailtemplatesTableComponent, canActivate: [RoleGuardService], data: {
          expectedRole: 'Admin'
        }
      }
    ]
  },
  {
    path: 'surveys', component: SidebarComponent, canActivate: [AuthGuard],
    children: [
      { path: '', component: SurveysTableComponent, canActivate: [AuthGuard] }
    ]
  },
  {
    path: 'SurveyAnswerForm/:id', component: SurveysAnswersFormComponent, canActivate: [AuthGuard],
  },
  {
    path: 'profile', component: SidebarComponent, canActivate: [AuthGuard],
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
