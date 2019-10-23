import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class EnumToStringConverterService {

  constructor() { }

  determineHolidayType(type: number): string {
    switch (type) {
      case 0:
        return 'Annual';

      case 1:
        return 'Parental';

      case 2:
        return 'Science';
    }
  }

  determineHolidayStatus(status: number): string {
    switch (status) {
      case 0:
        return 'Unconfirmed';

      case 1:
        return 'Declined';

      case 2:
        return 'Confirmed';
    }
  }
}
