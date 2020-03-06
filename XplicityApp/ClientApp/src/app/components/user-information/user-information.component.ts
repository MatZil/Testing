import { Component, OnInit, Input } from '@angular/core';
import { TableRowUserModel } from '../../models/table-row-user-model';
import { UserService } from '../../services/user.service';
import { NotificationSettings } from 'src/app/models/notification-settings';
import { MatSliderChange } from '@angular/material';
import { NotificationSettingsService } from 'src/app/services/notification-settings.service';
import { AuthenticationService } from 'src/app/services/authentication.service';

@Component({
  selector: 'app-user-information',
  templateUrl: './user-information.component.html',
  styleUrls: ['./user-information.component.scss']
})
export class UserInformation implements OnInit {

  @Input()
  checked: Boolean

  currentUser: TableRowUserModel;
  currentUserSettings: NotificationSettings = new NotificationSettings();


  constructor(
    private userService: UserService,
    private notificationSettingsService: NotificationSettingsService,
    private authService: AuthenticationService
  ) { }

  ngOnInit() {
    this.userService.getCurrentUser().subscribe(user => {
      this.currentUser = user;
      this.currentUser.role = this.userService.getRole();
    });

    this.notificationSettingsService.getUserNotificationSettings(this.authService.getUserId()).subscribe(
      data => {
        this.currentUserSettings = Object.assign({}, data);
        console.log(this.currentUserSettings);
      }
    )
  }

  onInputChange1(value: boolean) {
    this.currentUserSettings.receiveBirthdayNotifications = value;

    this.updateSettings();
  }

  onInputChange2(value: boolean) {
    this.currentUserSettings.broadcastOwnBirthday = value;

    this.updateSettings();
  }

  updateSettings() {
    this.notificationSettingsService.updateUserNotificationSettings(this.authService.getUserId(), this.currentUserSettings).subscribe(
      error => {
        console.log(error);
      }
    )
  }
}
