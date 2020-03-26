import { InventoryCategory } from './inventory-category';
import { Tag } from './tag';

export class InventoryItem {
  id: number;
  name: string;
  serialNumber: string;
  originalPrice: number;
  currentPrice: number;
  purchaseDate: Date;
  expiryDate: Date;
  comment: string;
  category: InventoryCategory;
  assignedTo: string;
  employeeId: number;
  archived: boolean;
  tags: Tag[];
}
