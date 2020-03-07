import { Component, OnInit } from '@angular/core';
import { UserService } from '../../services/user.service';
import { MatDialog } from '@angular/material';
import { UserPasswordFormComponent } from '../user-password-form/user-password-form.component';
import { UploadComponent } from '../upload/upload.component';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { NotificationSettingsService } from 'src/app/services/notification-settings.service';
import { NotificationSettings } from 'src/app/models/notification-settings';

@Component({
  selector: 'app-user-settings',
  templateUrl: './user-settings.component.html',
  styleUrls: ['./user-settings.component.scss']
})
export class UserSettings implements OnInit {
  currentUserSettings: NotificationSettings = new NotificationSettings();

  constructor(
    private userService: UserService,
    public dialog: MatDialog,
    private notificationSettingsService: NotificationSettingsService,
    private authenticationService: AuthenticationService
  ) { }

  ngOnInit() {
    this.loadCurrentUserSettings();
  }

  showUploadModal(): void {
    const dialogRef = this.dialog.open(UploadComponent, {
      width: '500px'
    });
  }

  closeUploadModal(): void {
    this.dialog.closeAll();
  }

  closePasswordModal(): void {
    this.dialog.closeAll();
  }

  showPasswordModal(): void {
    const dialogRef = this.dialog.open(UserPasswordFormComponent, {
      width: '500px'
    });
  }

  isAdmin(): boolean {
    return this.userService.isAdmin();
  }

  updateNotificationSettings(): void {
    this.notificationSettingsService.updateNotificationSettings(this.authenticationService.getUserId(), this.currentUserSettings).subscribe();
  }

  loadCurrentUserSettings(): void {
    this.notificationSettingsService.getNotificationSettings(this.authenticationService.getUserId()).subscribe(
      data => {
        this.currentUserSettings = Object.assign({}, data);
      }
    )
  }
}
