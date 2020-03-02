import { Tag } from "./tag";

export class NewInventoryItem {
  name: string;
  serialNumber: string;
  purchaseDate: Date;
  expiryDate: Date;
  comment: string;
  tags: Tag[];
}
