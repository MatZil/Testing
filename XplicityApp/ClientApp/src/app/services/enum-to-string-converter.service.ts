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

      case HolidayStatus.Rejected:
        return 'Rejected';

      case HolidayStatus.Confirmed:
        return 'Confirmed';
    }
  }

  determineHolidayStatusColor(status: number): string {
    switch (status) {
      case HolidayStatus.Pending:
        return 'orange';

      case HolidayStatus.ClientConfirmed:
        return 'orange';

      case HolidayStatus.Rejected:
        return 'red';

      case HolidayStatus.Confirmed:
        return 'green';
    }
  }
}
