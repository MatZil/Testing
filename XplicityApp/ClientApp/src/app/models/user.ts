import { Updateuser } from './updateuser';

export class User extends Updateuser {
    id: number;
    overtimeDays: number;
    nextOvertimeHours: number;
    nextOvertimeMinutes: number;
    currentAvailableLeaves: number;
    nextMonthAvailableLeaves: number;
}
