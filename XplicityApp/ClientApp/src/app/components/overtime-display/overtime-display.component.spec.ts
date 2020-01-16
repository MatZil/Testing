import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OvertimeDisplayComponent } from './overtime-display.component';

describe('OvertimeDisplayComponent', () => {
  let component: OvertimeDisplayComponent;
  let fixture: ComponentFixture<OvertimeDisplayComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OvertimeDisplayComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OvertimeDisplayComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
