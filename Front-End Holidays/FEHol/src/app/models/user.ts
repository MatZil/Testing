import { EmployeeStatus } from './employee-status.enum';

export class User {
    id: number;
    name: string;
    surname: string;
    clientId: number;
    worksFromDate: Date;
    birthdayDate: Date;
    daysOfVacation: number;
    freeWorkDays: number;
    parentalLeaveLimit: number;
    currentAvailableLeaves: number;
    nextMonthAvailableLeaves: number;
    email: string;
    role: string;
    position: string;
    healthCheckDate: Date;
    status: EmployeeStatus;
}
