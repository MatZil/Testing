import { Component, OnInit, Inject} from '@angular/core';
import { NewInventoryItem } from 'src/app/models/new-inventory-item';
import { FormGroup, FormBuilder } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { AddModalData } from '../inventory-add-form/add-modal-data';

@Component({
  selector: 'app-add-inventory-form',
  templateUrl: './inventory-add-form.component.html',
  styleUrls: ['./inventory-add-form.component.scss'],

})
export class AddInventoryFormComponent implements OnInit {
  addInventoryItemForm: FormGroup;
  newInventoryItem = new NewInventoryItem();

  constructor(
    private formBuilder: FormBuilder,
    public dialogRef: MatDialogRef<AddInventoryFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: AddModalData
  ) {
  }

  ngOnInit() {
    this.initializeFormGroup();
    
  }

  initializeFormGroup() {
    this.addInventoryItemForm = this.formBuilder.group({
      baseForm: [this.newInventoryItem]
    });
  }

  isFormInvalid(baseForm: FormGroup): boolean {
    return !(baseForm.valid && this.addInventoryItemForm.valid);
  }

  onSubmit(): void {
    const newInventoryItem = this.getFormInventoryItem();
    this.dialogRef.close(newInventoryItem);
  }

  getFormInventoryItem(): NewInventoryItem {
    const formInventoryItem = Object.assign({}, this.addInventoryItemForm.value, this.addInventoryItemForm.controls.baseForm.value);
    return formInventoryItem;
  }
}

