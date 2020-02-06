import { InventoryCategory } from './inventory-category';
import { BaseUser } from './base-user';

export class InventoryItem {
    id: number;
    name: string;
    serialNumber: string;
    price: number;
    purchaseDate: Date;
    expiryDate: Date;
    comment: string;
    category: InventoryCategory;
    employee: BaseUser;
}
