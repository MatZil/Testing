import { HolidayStatus } from '../enums/holidayStatus';
import { NewHoliday } from './new-holiday';

export class Holiday extends NewHoliday {
    id: number;
    overtimeHours: number;
    status: HolidayStatus;
    requestCreatedDate: Date;
}
