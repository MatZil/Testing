import { Injectable } from '@angular/core';
import { HolidayType } from '../enums/holidayType';
import { HolidayStatus } from '../enums/holidayStatus';
import { SurveyType } from '../enums/surveyType';

@Injectable({
  providedIn: 'root'
})
export class EnumToStringConverterService {

  constructor() { }

  determineSurveyType(type: number): string {
    switch (type) {
      case SurveyType.Multiple_choice:
        return 'Multiple choice';

      case SurveyType.Likert_scale:
        return 'Likert scale';

      case SurveyType.Ranking:
        return 'Ranking';

      case SurveyType.Text_entry:
        return 'Text entry';
    }
  }

  determineHolidayType(type: number): string {
    switch (type) {
      case HolidayType.Annual:
        return 'Annual';

      case HolidayType.DayForChildren:
        return 'Day for children';

      case HolidayType.Science:
        return 'Science';

      case HolidayType.Unpaid:
        return 'Unpaid';
    }
  }

  determineHolidayStatus(status: number): string {
    switch (status) {
      case HolidayStatus.Pending:
        return 'Pending';

      case HolidayStatus.Abandoned:
        return 'Abandoned';

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

      case HolidayStatus.Abandoned:
        return 'red';

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
