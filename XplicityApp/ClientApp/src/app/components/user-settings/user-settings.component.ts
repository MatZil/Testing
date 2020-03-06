import { Component, OnInit } from '@angular/core';
import { UserService } from '../../services/user.service';
import { MatDialog } from '@angular/material';
import { UserPasswordFormComponent } from '../user-password-form/user-password-form.component';
import { UploadComponent } from '../upload/upload.component';

@Component({
  selector: 'app-user-settings',
  templateUrl: './user-settings.component.html',
  styleUrls: ['./user-settings.component.scss']
})
export class UserSettings implements OnInit {

  constructor(
    private userService: UserService,
    public dialog: MatDialog
  ) {
  }

  ngOnInit() {
  }

  showUploadModal() {
    const dialogRef = this.dialog.open(UploadComponent, {
      width: '500px'
    });
  }

  closeUploadModal() {
    this.dialog.closeAll();
  }
  closePasswordModal() {
    this.dialog.closeAll();
  }
  showPasswordModal() {
    const dialogRef = this.dialog.open(UserPasswordFormComponent, {
      width: '500px'
    });
  }

  isAdmin(): boolean {
    return this.userService.isAdmin();
  }
}
