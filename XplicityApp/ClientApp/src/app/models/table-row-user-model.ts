import { Updateuser } from './updateuser';

export class TableRowUserModel extends Updateuser {
    id: number;
    overtimeDays: number;
    currentAvailableLeaves: number;
    nextMonthAvailableLeaves: number;
}
