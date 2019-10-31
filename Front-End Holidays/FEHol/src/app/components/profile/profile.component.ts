import { Component, OnInit } from '@angular/core';
import { User } from '../../models/user';
import { AuthenticationService } from '../../services/authentication-service.service';
import { UserService } from '../../services/user.service';
import { NgForm } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {
  currentUser: User;
  constructor(
    private http: HttpClient,
    private authenticationService: AuthenticationService,
    private userService: UserService,
  ) {
  }

  ngOnInit() {
    this.userService.getUser(this.authenticationService.getUserId()).subscribe(user => {
      this.currentUser = user;
    });

  }
  changePassword(form: NgForm) {
    const id = this.authenticationService.getUserId();
    this.http.post(`${environment.webApiUrl}/Employees/${id}/Change_password`, form.value).subscribe(
      data => {
        form.resetForm();
      },
      error => {
        console.log(error);
      }
    );
  }
}
