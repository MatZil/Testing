import { Component, OnInit } from '@angular/core';
import { PolicyService } from '../../services/policy.service';
import { AuthenticationService } from '../../services/authentication-service.service';
import { User } from '../../models/user';

@Component({
  selector: 'app-policy',
  templateUrl: './policy.component.html',
  styleUrls: ['./policy.component.scss']
})
export class PolicyComponent implements OnInit {
  holidaysCount: number;
  user: User;
  image: string;
  constructor(private policyService: PolicyService, private authenticationService: AuthenticationService
  ) {
    this.image = 'assets/bg.jpg';
  }

  ngOnInit() {
    this.getCount(this.authenticationService.getUserId());
  }

  getCount(userId: number) {
    this.policyService.getHolidaysCount(userId).subscribe(count => {
      this.holidaysCount = count;
    });
  }
}
