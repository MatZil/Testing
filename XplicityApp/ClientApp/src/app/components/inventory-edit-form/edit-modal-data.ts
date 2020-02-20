import { InventoryItem } from 'src/app/models/inventory-item';
import { AddModalData } from '../inventory-add-form/add-modal-data';
import { NewTag } from 'src/app/models/new-tag';

export interface EditModalData extends AddModalData {
  inventoryItemToUpdate: InventoryItem;
  tags: NewTag[];
}

