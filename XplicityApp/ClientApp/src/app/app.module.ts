import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NgZorroAntdModule, NZ_I18N, en_US } from 'ng-zorro-antd';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { registerLocaleData } from '@angular/common';
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
import { MatDatepickerModule, MatNativeDateModule, MatRadioModule, MatCheckboxModule } from '@angular/material';
import { MatDialogModule } from '@angular/material/dialog';
import { ErrorPageComponent } from './components/error-page/error-page.component';
import { EmployeesTableComponent } from './components/employees-table/employees-table.component';
import { HomeComponent } from './components/home/home.component';
import { ProfileComponent } from './components/profile/profile.component';
import { HolidaysTableComponent } from './components/holidays-table/holidays-table.component';
import { PdfComponent } from './components/pdf/pdf.component';
import { PdfViewerModule } from 'ng2-pdf-viewer';
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
registerLocaleData(en);
export function tokenGetter() {
  return localStorage.getItem("token");
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
    PdfComponent,
    EmailtemplatesTableComponent,
    UploadComponent,
    OvertimeDisplayComponent,
    InventoryTableComponent,
    AddEmployeeFormComponent,
    EditEmployeeFormComponent,
    BaseEmployeeFormComponent
  ],
  entryComponents: [AddEmployeeFormComponent, EditEmployeeFormComponent],
  imports: [
    BrowserModule,
    AppRoutingModule,
    NgZorroAntdModule,
    FormsModule,
    ReactiveFormsModule,
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
    PdfViewerModule,
    MatDialogModule,
    MatSnackBarModule,
    MatTabsModule,
    NzUploadModule,
    MatProgressBarModule,
    JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter,
        whitelistedDomains: ["http://localhost:4200"],
        blacklistedRoutes: ["example.com/examplebadroute/"]
      }
    })
  ],
  providers: [{ provide: NZ_I18N, useValue: en_US },
  [RoleGuardService],
  { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
  { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true }],
  bootstrap: [AppComponent]
})
export class AppModule {
}
