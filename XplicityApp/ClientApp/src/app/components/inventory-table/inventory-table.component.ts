import { Component, OnInit, Input, ViewChild, ElementRef } from '@angular/core';
import { InventoryItem } from 'src/app/models/inventory-item';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { InventoryService } from 'src/app/services/inventory.service';
import { InventoryCategory } from 'src/app/models/inventory-category';
import { InventoryCategoryService } from 'src/app/services/inventory-category.service';
import { TableRowUserModel } from 'src/app/models/table-row-user-model';
import { UserService } from 'src/app/services/user.service';

import { ENTER } from '@angular/cdk/keycodes';
import { MatChipInputEvent, MatAutocomplete, MatAutocompleteSelectedEvent, MatSnackBar } from '@angular/material';
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

  searchCategoryValue = '';
  searchOwnerValue = '';
  searchDateValue : Date;
  sortName: string | null = null;
  sortValue: string | null = null;
  listOfData: InventoryItem[] = [];

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
    private tagsService: TagService,
    private snackBar: MatSnackBar
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
        this.listOfData = [...this.equipment];
      });
    } else {
      this.inventoryService.getAllInventoryItems().subscribe(inventoryItems => {
        this.equipment = inventoryItems;
        this.listOfData = [...this.equipment];
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
    this.clearTagList();
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
    this.clearTagList();
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

  resetCategory(): void {
    this.searchCategoryValue = '';
    this.search();
  }
  resetOwner(): void {
    this.searchOwnerValue = '';
    this.search();
  }
  search(): void {
    const filterFunc = (item: InventoryItem) => {
      if(!this.searchDateValue){
        return (
          item.category.name.toUpperCase().indexOf(this.searchCategoryValue.toUpperCase()) !== -1 &&
          item.assignedTo.toUpperCase().indexOf(this.searchOwnerValue.toUpperCase()) !== -1
        );
      }
      else {
        if(!item.expiryDate)
          return false;
        else {
          var targetDate = Date.parse(this.searchDateValue.toDateString());
          var tempDate = new Date(item.expiryDate);
          var itemDate = Date.parse(tempDate.toDateString());
          return (
            item.category.name.toUpperCase().indexOf(this.searchCategoryValue.toUpperCase()) !== -1 &&
            item.assignedTo.toUpperCase().indexOf(this.searchOwnerValue.toUpperCase()) !== -1 &&
            targetDate == itemDate
          );
        }
      }

    };
    const data = this.listOfData.filter((item: InventoryItem) => filterFunc(item));
    this.equipment = data.sort((a, b) =>
      this.sortValue === 'ascend'
        ? a[this.sortName!] > b[this.sortName!]
          ? 1
          : -1
        : b[this.sortName!] > a[this.sortName!]
          ? 1
          : -1
    );
  }


  add(event: MatChipInputEvent): void {
    const input = event.input;
    const tagTitle = event.value;

    if ((tagTitle || '').trim() && this._validateTag(tagTitle)) {
      this.tagsService.createNewTag({ Title: tagTitle }).subscribe(id => {
        this.tagsSelected.push({ Id: Number(id), Title: tagTitle });
      }, error => {
          console.log(error);
      });
    } else {
      this.showWarningMessage('Tag is invalid!');
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
    this.tagsSelected.push({ Id: event.option.value, Title: event.option.viewValue });
    this.tagInput.nativeElement.value = '';
    this.tagsControl.setValue(null);
  }

  private _tagsFilter(value: string): Tag[] {
    this.tagsService.getAllByTitle(value).subscribe(data => {
      this.tagsAfterFiltration = data;
    });

    return this.tagsAfterFiltration;
  }

  showWarningMessage(message: string) {
    this.snackBar.open(message, 'Close', {
      duration: 3000,
    })
  }

  clearTagList() {
    this.tagsSelected = [];
  }

  private _validateTag(tagTitle: string): boolean {
    let regexp = new RegExp('^[a-zA-Z0-9#-]*$');

    if (tagTitle.length > 10 && tagTitle.length < 3 && !regexp.test(tagTitle)) {
      return false;
    }

    return true;
  }

}

