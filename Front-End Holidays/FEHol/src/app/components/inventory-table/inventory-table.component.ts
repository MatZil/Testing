import { Component, OnInit, Input } from '@angular/core';
import { InventoryItem } from 'src/app/models/inventory-item';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-inventory-table',
  templateUrl: './inventory-table.component.html',
  styleUrls: ['./inventory-table.component.scss']
})
export class InventoryTableComponent implements OnInit {
  equipment: InventoryItem[] = [];
  @Input()
  id: number;
  constructor(private userService: UserService) { }

  ngOnInit() {
    this.refreshTable(this.id);
  }
  refreshTable(id: number) {
    this.userService.getEquipment(id).subscribe(data => {
      this.equipment = data;
    });
  }
}
