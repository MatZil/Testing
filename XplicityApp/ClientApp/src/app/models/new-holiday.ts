import { HolidayType } from '../enums/holidayType';

export class NewHoliday {
    employeeId: number;
    type: HolidayType;
    fromInclusive: Date;
    toInclusive: Date;
    overtimeDays: number;
    paid: boolean;
}
