import { Component, OnInit, Inject } from '@angular/core';
import { InventoryItem } from 'src/app/models/inventory-item';
import { FormGroup, FormBuilder, FormControl} from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { EditModalData } from './edit-modal-data';

@Component({
  selector: 'app-edit-inventory-form',
  templateUrl: './inventory-edit-form.component.html',
  styleUrls: ['./inventory-edit-form.component.scss']
})
export class EditInventoryFormComponent implements OnInit {
  editInventoryForm: FormGroup;

  constructor(
    private formBuilder: FormBuilder,
    public dialogRef: MatDialogRef<EditInventoryFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: EditModalData) { }

  ngOnInit() {
    this.initializeFormGroup();
  }

  closeModal(returnValue: any): void {
    this.dialogRef.close(returnValue);
  }

  onSubmit(): void {
    const updateInventoryItem = this.getFormInventoryItem();
    this.dialogRef.close(updateInventoryItem);
  }

  getFormInventoryItem(): InventoryItem {
    const formInventoryItem = Object.assign({}, this.editInventoryForm.value, this.editInventoryForm.controls.baseForm.value);
    if (formInventoryItem.employeeId == 0) {
      formInventoryItem.employeeId = '';
    }
    return formInventoryItem;
  }

  initializeFormGroup(): void {
    this.editInventoryForm = this.formBuilder.group({
      baseForm: [this.data.inventoryItemToUpdate]
    });
  }

  isFormInvalid(baseForm: FormGroup): boolean {
    return !(baseForm.valid && this.editInventoryForm.valid);
  }

  archiveItem() {
    const updateInventoryItem = this.getFormInventoryItem();
    updateInventoryItem.archived = true;
    this.dialogRef.close(updateInventoryItem);
  }
}

