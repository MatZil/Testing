import { Component, OnInit } from '@angular/core';
import { TableRowUserModel } from '../../models/table-row-user-model';
import { UserService } from '../../services/user.service';
import { NotificationSettings } from 'src/app/models/notification-settings';
import { NotificationSettingsService } from 'src/app/services/notification-settings.service';
import { AuthenticationService } from 'src/app/services/authentication.service';

@Component({
  selector: 'app-user-information',
  templateUrl: './user-information.component.html',
  styleUrls: ['./user-information.component.scss']
})
export class UserInformation implements OnInit {
  currentUser: TableRowUserModel;
  currentUserSettings: NotificationSettings = new NotificationSettings();

  constructor(
    private userService: UserService,
    private notificationSettingsService: NotificationSettingsService,
    private authService: AuthenticationService
  ) { }

  ngOnInit() {
    this.loadCurrentUser();
    this.loadCurrentUserSettings();
  }

  onReceiveBirthdayNotificationsInputChange(settingValue: boolean) {
    this.currentUserSettings.receiveBirthdayNotifications = settingValue;
    this.updateNotificationSettings();
  }

  onBroadcastOwnBirthdayInputChange2(settingValue: boolean) {
    this.currentUserSettings.broadcastOwnBirthday = settingValue;
    this.updateNotificationSettings();
  }

  updateNotificationSettings() {
    this.notificationSettingsService.updateNotificationSettings(this.authService.getUserId(), this.currentUserSettings).subscribe()
  }

  loadCurrentUser(): void {
    this.userService.getCurrentUser().subscribe(user => {
      this.currentUser = user;
      this.currentUser.role = this.userService.getRole();
    });
  }

  loadCurrentUserSettings() {
    this.notificationSettingsService.getNotificationSettings(this.authService.getUserId()).subscribe(
      data => {
        this.currentUserSettings = Object.assign({}, data);
      }
    )
  }
}
