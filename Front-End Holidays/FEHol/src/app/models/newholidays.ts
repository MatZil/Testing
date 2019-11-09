import { HolidayStatus } from '../enums/holidayStatus';
import { HolidayType } from '../enums/holidayType';

export class Newholidays {
    employeeId: number;
    type: HolidayType;
    fromInclusive: Date;
    toExclusive: Date;
    isCofirmed: boolean;
    paid: boolean;
    status: HolidayStatus;
    requestCreatedDate: Date;
}
