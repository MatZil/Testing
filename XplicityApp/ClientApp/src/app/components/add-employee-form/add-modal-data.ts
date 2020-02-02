import { Role } from 'src/app/models/role';
import { Client } from 'src/app/models/client';

export interface AddModalData {
    roles: Role[];
    clients: Client[];
}
