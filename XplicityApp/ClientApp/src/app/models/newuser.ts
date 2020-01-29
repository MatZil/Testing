import { EmployeeStatus } from './employee-status.enum';

export class Newuser {
    name: string;
    surname: string;
    clientId: number;
    worksFromDate: Date;
    birthdayDate: Date;
    daysOfVacation: number;
    parentalLeaveLimit: number;
    overtimeHours: number;
    email: string;
    password: string;
    role: string;
    position: string;
    healthCheckDate: Date;
    status: EmployeeStatus;
    isManualHolidaysInput: boolean;
    freeWorkDays: number;
}
