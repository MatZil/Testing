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
      case HolidayStatus.Unconfirmed:
        return 'Unconfirmed';

      case HolidayStatus.Declined:
        return 'Declined';

      case HolidayStatus.Confirmed:
        return 'Confirmed';
    }
  }
}
