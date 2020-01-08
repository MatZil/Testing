import { TestBed } from '@angular/core/testing';

import { InventoryCategoryService } from './inventory-category.service';

describe('InventoryCategoryService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: InventoryCategoryService = TestBed.get(InventoryCategoryService);
    expect(service).toBeTruthy();
  });
});
