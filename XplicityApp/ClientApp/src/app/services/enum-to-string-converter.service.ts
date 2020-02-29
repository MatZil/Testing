import { Injectable } from '@angular/core';
import { HolidayType } from '../enums/holidayType';
import { HolidayStatus } from '../enums/holidayStatus';

@Injectable({
  providedIn: 'root'
})
export class EnumToStringConverterService {

  constructor() { }

  determineHolidayType(type: number): string {
    switch (type) {
      case HolidayType.Annual:
        return 'Annual';

      case HolidayType.Parental:
        return 'Parental';

      case HolidayType.Science:
        return 'Science';
    }
  }

  determineHolidayStatus(status: number): string {
    switch (status) {
      case HolidayStatus.Pending:
        return 'Pending';

      case HolidayStatus.ClientConfirmed:
        return 'Confirmed by client';

      case HolidayStatus.ClientRejected:
        return 'Rejected by client';

      case HolidayStatus.AdminRejected:
        return 'Rejected by admin';

      case HolidayStatus.AdminConfirmed:
        return 'Confirmed by admin';
    }
  }

  determineHolidayStatusColor(status: number): string {
    switch (status) {
      case HolidayStatus.Pending:
        return 'orange';

      case HolidayStatus.ClientConfirmed:
        return 'orange';

      case HolidayStatus.AdminRejected:
        return 'red';

      case HolidayStatus.ClientRejected:
        return 'red';

      case HolidayStatus.AdminConfirmed:
        return 'green';
    }
  }
}
