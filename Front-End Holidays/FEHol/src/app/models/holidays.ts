import { HolidayType } from '../enums/holidayType';
import { HolidayStatus } from '../enums/holidayStatus';

export class Holidays {
    id: number;
    employeeId: number;
    type: HolidayType;
    fromInclusive: Date;
    toExclusive: Date;
    isCofirmed: boolean;
    paid: boolean;
    status: HolidayStatus;
    requestCreatedDate: Date;
}
