import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmailtemplatesTableComponent } from './email-templates-table.component';

describe('EmailtemplatesTableComponent', () => {
  let component: EmailtemplatesTableComponent;
  let fixture: ComponentFixture<EmailtemplatesTableComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmailtemplatesTableComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmailtemplatesTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
