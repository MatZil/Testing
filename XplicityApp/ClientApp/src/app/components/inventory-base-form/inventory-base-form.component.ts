import { Component, OnInit, Input, forwardRef, ViewChild, ElementRef } from '@angular/core';
import { InventoryItem } from 'src/app/models/inventory-item';
import { FormGroup, FormBuilder, Validators, NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';
import { InventoryService } from 'src/app/services/inventory.service';
import { InventoryCategory } from 'src/app/models/inventory-category';
import { InventoryCategoryService } from 'src/app/services/inventory-category.service';
import { UserService } from 'src/app/services/user.service';
import { BaseUser } from 'src/app/models/base-user';
import { DatePipe } from '@angular/common';

import { ENTER } from '@angular/cdk/keycodes';
import { MatChipInputEvent, MatAutocomplete, MatAutocompleteSelectedEvent, MatSnackBar } from '@angular/material';
import { TagService } from '../../services/tag.service';
import { Tag } from '../../models/tag';
import { FormControl } from '@angular/forms';
import { startWith, map } from 'rxjs/operators';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-base-inventory-form',
  templateUrl: './inventory-base-form.component.html',
  styleUrls: ['./inventory-base-form.component.scss'],
    providers: [
    {
      provide: NG_VALUE_ACCESSOR,
        useExisting: forwardRef(() => BaseInventoryFormComponent),
      multi: true
    }
  ],
  exportAs: 'baseInventoryFormComponent'
})

export class BaseInventoryFormComponent implements OnInit, ControlValueAccessor {
  baseForm: FormGroup;
  equipment: InventoryItem[] = [];
  @Input() categories: InventoryCategory[];
  @Input() employees: BaseUser[];

  readonly separatorKeysCodes: number[] = [ENTER];
  tagsControl = new FormControl();
  tagsSelected: Tag[] = [];
  tagsAfterFiltration: Tag[] = [];
  tagSuggestions: Observable<Tag[]>;

  @ViewChild('tagInput', { static: false }) tagInput: ElementRef<HTMLInputElement>;
  @ViewChild('auto', { static: false }) matAutocomplete: MatAutocomplete;

  constructor(
    private formBuilder: FormBuilder,
    private categoryService: InventoryCategoryService,
    private userService: UserService,
    private datePipe: DatePipe,
    private tagsService: TagService,
    private snackBar: MatSnackBar
  ) {
    this.tagSuggestions = this.tagsControl.valueChanges.pipe(
      startWith(null),
      map((tagTitle: string | null) => tagTitle ? this._tagsFilter(tagTitle) : this.tagsAfterFiltration.slice()));
  }

  ngOnInit() {
    this.initializeFormGroup();
  }

  initializeFormGroup() {
    this.baseForm = this.formBuilder.group({
      name: ['',
        [Validators.required]],
      serialNumber: ['',
        [Validators.required]],
      price: ['', [Validators.required]],
      purchaseDate: ['',
        [Validators.required]],
      expiryDate: [''],
      inventoryCategoryId: ['',
        [Validators.required]],
      category: [''],
      assignedTo: [''],
      comment: [''],
      archived: [false],
      tags :[null],
      employeeId: ['']
    });
    this.onCategoryChange();
    this.stripTimeFromDates();

  }

  stripTimeFromDates() {
    this.baseForm.controls.purchaseDate.valueChanges.subscribe(value => {
      this.baseForm.controls.purchaseDate.setValue(this.datePipe.transform(value, 'yyyy-MM-dd'), { emitEvent: false });
    });
    this.baseForm.controls.expiryDate.valueChanges.subscribe(value => {
      this.baseForm.controls.expiryDate.setValue(this.datePipe.transform(value, 'yyyy-MM-dd'), { emitEvent: false });
    });
  }

  writeValue(value: any) {
    if (value) {
      this.baseForm.patchValue(value);
    }
  }

  registerOnChange(fn: any) {
    this.baseForm.valueChanges.subscribe(fn);
  }

  registerOnTouched(fn: any) { }

  onCategoryChange() {
    this.baseForm.get('inventoryCategoryId').valueChanges.subscribe(selectedCategoryId => {
      const licenseCategoryId = 4;
      if (selectedCategoryId !== licenseCategoryId) {
        this.baseForm.get('expiryDate').reset();
        this.baseForm.get('expiryDate').disable();
      } else {
        this.baseForm.get('expiryDate').enable();
      }
    });
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

  add(event: MatChipInputEvent): void {
    const input = event.input;
    const tagTitle = event.value;

    if (this._validateTag(tagTitle)) {
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

    if (tagTitle.length > 10) {
      return false;
    }
    if (tagTitle.length < 3) {
      return false;
    }
    if (!regexp.test(tagTitle)) {
      return false;
    }

    return true;
  }

  saveTags() {
    return this.tagsSelected.filter(x => x.Id);
  }
}

