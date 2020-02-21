import { BaseUser } from 'src/app/models/base-user';
import { InventoryCategory } from 'src/app/models/inventory-category';
import { NewTag } from 'src/app/models/new-tag';

export interface AddModalData {
  employees: BaseUser[];
  categories: InventoryCategory[];
  tags: NewTag[];
}
