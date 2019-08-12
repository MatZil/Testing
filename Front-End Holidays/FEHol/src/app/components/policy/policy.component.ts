import { Component, OnInit } from '@angular/core';
import { PolicyService } from '../../services/policy.service';
import { AuthenticationService } from '../../../app/services/authentication-service.service';
import { User } from '../../models/user';

@Component({
  selector: 'app-policy',
  templateUrl: './policy.component.html',
  styleUrls: ['./policy.component.scss']
})
export class PolicyComponent implements OnInit {
  holidaysCount: number;
  user: User;

  constructor(private policyService: PolicyService, private authenticationService: AuthenticationService
  ) {
    this.authenticationService.currentUser.subscribe(currentUser => this.user = currentUser);
  }

  ngOnInit() {
    this.getCount(this.user.id);
  }

  getCount(userId: number) {
    this.policyService.getHolidaysCount(userId).subscribe(count => {
      this.holidaysCount = count;
    });
  }
}
