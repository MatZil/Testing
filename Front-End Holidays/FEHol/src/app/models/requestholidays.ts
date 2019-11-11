import { HolidayType } from '../enums/holidayType';

export class Requestholidays {
    employeeId: number;
    type: HolidayType;
    fromInclusive: Date;
    toExclusive: Date;
    paid: boolean;
}
