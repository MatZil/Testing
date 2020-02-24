import { Component, OnInit } from '@angular/core';
import { TableRowUserModel } from '../../models/table-row-user-model';
import { UserService } from '../../services/user.service';
import { FormGroup } from '@angular/forms';
import { FileType } from '../../enums/fileType';

@Component({
  selector: 'app-user-information',
  templateUrl: './user-information.component.html',
  styleUrls: ['./user-information.component.scss']
})
export class UserInformation implements OnInit {
  currentUser: TableRowUserModel;
  hideOldPassword = true;
  hideFirstPassword = true;
  hideSecondPassword = true;
  registerForm: FormGroup;
  isVisibleUploadModal = false;
  isVisiblePasswordModal = false;
  fileTypes = FileType;

  constructor(
    private userService: UserService) { }

  ngOnInit() {
    this.userService.getCurrentUser().subscribe(user => {
      this.currentUser = user;
      this.currentUser.role = this.userService.getRole();
    });
  }
}
