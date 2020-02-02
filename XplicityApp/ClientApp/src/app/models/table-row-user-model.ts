import { Updateuser } from './updateuser';

export class TableRowUserModel extends Updateuser {
    id: number;
    overtimeDays: number;
    nextOvertimeHours: number;
    nextOvertimeMinutes: number;
    currentAvailableLeaves: number;
    nextMonthAvailableLeaves: number;
}
