import { HolidayStatus } from '../enums/holidayStatus';
import { HolidayType } from '../enums/holidayType';

export class Newholidays {
    employeeId: number;
    type: HolidayType;
    fromInclusive: Date;
    toExclusive: Date;
    overtimeDays: number;
    isCofirmed: boolean;
    paid: boolean;
    status: HolidayStatus;
    requestCreatedDate: Date;
}
