import { Updateuser } from 'src/app/models/updateuser';
import { AddModalData } from '../add-employee-form/add-modal-data';

export interface EditModalData extends AddModalData {
    userToUpdate: Updateuser;
    isEditingSelf: boolean;
}
