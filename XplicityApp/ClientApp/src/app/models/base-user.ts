import { EmployeeStatus } from './employee-status.enum';

export abstract class BaseUser {
    name: string;
    surname: string;
    clientId: number;
    worksFromDate: Date;
    birthdayDate: Date;
    daysOfVacation: number;
    parentalLeaveLimit: number;
    email: string;
    role: string;
    position: string;
    healthCheckDate: Date;
    status: EmployeeStatus;
    freeWorkDays: number;
}
