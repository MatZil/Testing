import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NgZorroAntdModule, NZ_I18N, en_US } from 'ng-zorro-antd';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { registerLocaleData, DatePipe } from '@angular/common';
import en from '@angular/common/locales/en';
import { SidebarComponent } from './components/sidebar/sidebar.component';
import { CalendarComponent } from './components/calendar/calendar.component';
import { AlertComponent } from './components/alert/alert.component';
import { LoginComponent } from './components/login/login.component';
import { ErrorInterceptor } from './helpers/error-interceptor';
import { ClientTableComponent } from './components/client-table/client-table.component';
import { JwtInterceptor } from './helpers/jwt-interceptor';
import { JwtModule } from '@auth0/angular-jwt';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatChipsModule } from '@angular/material/chips';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatTableModule } from '@angular/material/table';
import { MatToolbarModule } from '@angular/material/toolbar';
import {
  MatDatepickerModule,
  MatNativeDateModule,
  MatRadioModule,
  MatCheckboxModule
} from '@angular/material';
import { MatDialogModule } from '@angular/material/dialog';
import { ErrorPageComponent } from './components/error-page/error-page.component';
import { EmployeesTableComponent } from './components/employees-table/employees-table.component';
import { HomeComponent } from './components/home/home.component';
import { ProfileComponent } from './components/profile/profile.component';
import { HolidaysTableComponent } from './components/holidays-table/holidays-table.component';
import { PolicyComponent } from './components/policy/policy.component';
import { PdfJsViewerModule } from 'ng2-pdfjs-viewer';
import { EmailtemplatesTableComponent } from './components/emailtemplates-table/emailtemplates-table.component';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { RoleGuardService } from './helpers/role-guard';
import { MatTabsModule } from '@angular/material/tabs';
import { NzUploadModule } from 'ng-zorro-antd/upload';
import { UploadComponent } from './components/upload/upload.component';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { InventoryTableComponent } from './components/inventory-table/inventory-table.component';
import { OvertimeDisplayComponent } from './components/overtime-display/overtime-display.component';
import { AddEmployeeFormComponent } from './components/add-employee-form/add-employee-form.component';
import { EditEmployeeFormComponent } from './components/edit-employee-form/edit-employee-form.component';
import { BaseEmployeeFormComponent } from './components/base-employee-form/base-employee-form.component';
import { AddInventoryFormComponent } from './components/inventory-add-form/inventory-add-form.component';
import { EditInventoryFormComponent } from './components/inventory-edit-form/inventory-edit-form.component';
import { BaseInventoryFormComponent } from './components/inventory-base-form/inventory-base-form.component';
import { ClientFormComponent } from './components/client-form/client-form.component';
import { HolidayRequestFormComponent } from './components/holiday-request-form/holiday-request-form.component';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
registerLocaleData(en);
export function tokenGetter() {
  return localStorage.getItem('token');
}
@NgModule({
  declarations: [
    AppComponent,
    SidebarComponent,
    CalendarComponent,
    AlertComponent,
    LoginComponent,
    ClientTableComponent,
    ErrorPageComponent,
    EmployeesTableComponent,
    HomeComponent,
    ProfileComponent,
    HolidaysTableComponent,
    PolicyComponent,
    EmailtemplatesTableComponent,
    UploadComponent,
    OvertimeDisplayComponent,
    InventoryTableComponent,
    AddEmployeeFormComponent,
    EditEmployeeFormComponent,
    BaseEmployeeFormComponent,
    AddInventoryFormComponent,
    EditInventoryFormComponent,
    BaseInventoryFormComponent,
    ClientFormComponent,
    HolidayRequestFormComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    NgZorroAntdModule,
    FormsModule,
    ReactiveFormsModule.withConfig({ warnOnNgModelWithFormControl: 'never' }),
    HttpClientModule,
    BrowserAnimationsModule,
    MatInputModule,
    MatCheckboxModule,
    MatButtonModule,
    MatCardModule,
    MatChipsModule,
    MatIconModule,
    MatSelectModule,
    MatTableModule,
    MatToolbarModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatRadioModule,
    PdfJsViewerModule,
    MatDialogModule,
    MatSnackBarModule,
    MatTabsModule,
    NzUploadModule,
    MatProgressBarModule,
    MatAutocompleteModule,
    JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter,
        whitelistedDomains: ['http://localhost:4200'],
        blacklistedRoutes: ['example.com/examplebadroute/']
      }
    })
  ],
  entryComponents: [
    AddEmployeeFormComponent,
    EditEmployeeFormComponent,
    AddInventoryFormComponent,
    EditInventoryFormComponent,
    ClientFormComponent,
    HolidayRequestFormComponent
  ],
  providers: [
    { provide: NZ_I18N, useValue: en_US },
    [RoleGuardService],
    [DatePipe],
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
