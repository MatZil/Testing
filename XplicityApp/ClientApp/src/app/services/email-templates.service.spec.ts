import { TestBed } from '@angular/core/testing';

import { EmailTemplatesService } from './email-templates.service';

describe('EmailTemplatesService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: EmailTemplatesService = TestBed.get(EmailTemplatesService);
    expect(service).toBeTruthy();
  });
});
