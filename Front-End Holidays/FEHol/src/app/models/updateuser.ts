import { EmployeeStatus } from './employee-status.enum';

export class Updateuser {
    name: string;
    surname: string;
    clientId: number;
    worksFromDate: Date;
    birthdayDate: Date;
    daysOfVacation: number;
    email: string;
    role: string;
    position: string;
    healthCheckDate: Date;
    status: EmployeeStatus;
}
