import { HolidayType } from '../enums/holidayType';

export class Requestholidays {
    employeeId: number;
    type: HolidayType;
    fromInclusive: Date;
    toInclusive: Date;
    overtimeDays: number;
    paid: boolean;
}
