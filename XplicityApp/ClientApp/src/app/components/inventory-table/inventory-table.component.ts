import { Component, OnInit, Input, ViewChild, ElementRef } from '@angular/core';
import { InventoryItem } from 'src/app/models/inventory-item';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { InventoryService } from 'src/app/services/inventory.service';
import { InventoryCategory } from 'src/app/models/inventory-category';
import { InventoryCategoryService } from 'src/app/services/inventory-category.service';
import { TableRowUserModel } from 'src/app/models/table-row-user-model';
import { UserService } from 'src/app/services/user.service';

import { ENTER } from '@angular/cdk/keycodes';
import { MatChipInputEvent, MatAutocomplete, MatAutocompleteSelectedEvent } from '@angular/material';
import { TagService } from '../../services/tag.service';
import { Tag } from '../../models/tag';
import { FormControl } from '@angular/forms';
import { startWith, map } from 'rxjs/operators';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-inventory-table',
  templateUrl: './inventory-table.component.html',
  styleUrls: ['./inventory-table.component.scss']
})
export class InventoryTableComponent implements OnInit {
  equipment: InventoryItem[] = [];
  @Input()
  employeeId: number;
  isVisibleNewItemModal = false;
  input: FormGroup;
  categories: InventoryCategory[] = [];
  employees: TableRowUserModel[] = [];
  selectedEmployee;
  isModifying = false;

  readonly separatorKeysCodes: number[] = [ENTER];
  tagsControl = new FormControl();
  tagsSelected: Tag[] = [];
  tagsAfterFiltration: Tag[] = [];
  tagSuggestions: Observable<Tag[]>;

  @ViewChild('tagInput', { static: false }) tagInput: ElementRef<HTMLInputElement>;
  @ViewChild('auto', { static: false }) matAutocomplete: MatAutocomplete;

  constructor(
    private inventoryService: InventoryService,
    private formBuilder: FormBuilder,
    private categoryService: InventoryCategoryService,
    private userService: UserService,
    private tagsService: TagService
  ) {
    this.tagSuggestions = this.tagsControl.valueChanges.pipe(
      startWith(null),
      map((tagTitle: string | null) => tagTitle ? this._tagsFilter(tagTitle) : this.tagsAfterFiltration.slice()));
  }

  ngOnInit() {
    this.input = this.formBuilder.group({
      id: [null],
      name: [null, [Validators.required]],
      serialNumber: [null, [Validators.required]],
      price: [null, [Validators.required]],
      purchaseDate: [null, [Validators.required]],
      expiryDate: [null, [Validators.required]],
      category: [null],
      assignedTo: [null],
      comment: [null],
      archived: [false],
      inventoryCategoryId: [null, [Validators.required]],
      employeeId: [null]
    });
    this.getAllUsers();
    this.getCategoriesList();
    this.refreshTable(this.employeeId);
    this.onCategoryChange();

  }
  onCategoryChange() {
    this.input.get('inventoryCategoryId').valueChanges.subscribe(selectedCategoryId => {
      const licenseCategoryId = 4;
      if (selectedCategoryId !== licenseCategoryId) {
        this.input.get('expiryDate').reset();
        this.input.get('expiryDate').disable();
      } else {
        this.input.get('expiryDate').enable();
      }
    });
  }
  refreshTable(id: number) {
    if (id) {
      this.inventoryService.getEquipmentByEmployeeId(id).subscribe(inventoryItems => {
        this.equipment = inventoryItems;
      });
    } else {
      this.inventoryService.getAllInventoryItems().subscribe(inventoryItems => {
        this.equipment = inventoryItems;
      });
    }
  }
  showNewItemModal() {
    this.isVisibleNewItemModal = true;
    this.selectedEmployee = this.employeeId;
  }

  closeNewItemModal() {
    this.isVisibleNewItemModal = false;
    this.isModifying = false;
    this.input.reset();
  }

  saveInventoryItem() {
    if (!this.isModifying) {
      this.inventoryService.createNewInventoryItem(this.input.value).subscribe(() => {
        this.refreshTable(this.employeeId);

      });
    } else {
      this.inventoryService.updateInventoryItem(this.input.value.id, this.input.value).subscribe(() => {
        this.refreshTable(this.employeeId);
      });
    }
    this.isVisibleNewItemModal = false;
    this.isModifying = false;
    this.input.reset();
  }

  getCategoriesList() {
    this.categoryService.getAllCategories().subscribe(categories => {
      this.categories = categories;
    });
  }

  getAllUsers() {
    this.userService.getAllUsers().subscribe(users => {
      this.employees = users;
    });
  }

  modify(data) {
    this.input.setValue(data);
    this.selectedEmployee = this.employeeId;
    this.isModifying = true;
    this.isVisibleNewItemModal = true;
  }
  archiveItem() {
    this.input.controls.archived.setValue(true);
    this.saveInventoryItem();
  }

  add(event: MatChipInputEvent): void {
    const input = event.input;
    const tagTitle = event.value;

    if ((tagTitle || '').trim()) {
      this.tagsService.createNewTag({ Title: tagTitle }).subscribe(id => {
        this.tagsSelected.push({ Id: Number(id), Title: tagTitle });
      });
    }

    if (input) {
      input.value = '';
    }

    this.tagsControl.setValue(null);
  }

  remove(tag: Tag): void {
    const index = this.tagsSelected.indexOf(tag);

    if (index >= 0) {
      this.tagsSelected.splice(index, 1);
    }
  }

  selected(event: MatAutocompleteSelectedEvent): void {
    let tagObject = event.option.viewValue.split(' ', 2);
    this.tagsSelected.push({ Id: Number(tagObject[1]), Title: tagObject[0] });
    this.tagInput.nativeElement.value = '';
    this.tagsControl.setValue(null);
  }

  private _tagsFilter(value: string): Tag[] {
    this.tagsService.getAllByFilter(value).subscribe(data => {
      this.tagsAfterFiltration = data;
    });

    return this.tagsAfterFiltration;
  }

}

