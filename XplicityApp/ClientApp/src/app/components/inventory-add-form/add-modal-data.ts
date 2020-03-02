import { BaseUser } from 'src/app/models/base-user';
import { InventoryCategory } from 'src/app/models/inventory-category';
import { InventoryItem } from '../../models/inventory-item';
import { Tag } from '../../models/tag';

export interface AddModalData {
  employees: BaseUser[];
  categories: InventoryCategory[];
  equipment: InventoryItem[];
  tags: Tag[];
}
