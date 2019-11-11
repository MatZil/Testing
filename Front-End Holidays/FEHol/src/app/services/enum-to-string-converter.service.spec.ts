import { TestBed } from '@angular/core/testing';

import { EnumToStringConverterService } from './enum-to-string-converter.service';

describe('EnumToStringConverterService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: EnumToStringConverterService = TestBed.get(EnumToStringConverterService);
    expect(service).toBeTruthy();
  });
});
