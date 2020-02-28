import { InventoryItem } from 'src/app/models/inventory-item';
import { AddModalData } from '../inventory-add-form/add-modal-data';

export interface EditModalData extends AddModalData {
  inventoryItemToUpdate: InventoryItem;
}

