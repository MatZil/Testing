import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { HolidaysTableComponent } from './holidays-table.component';

describe('HolidaysTableComponent', () => {
  let component: HolidaysTableComponent;
  let fixture: ComponentFixture<HolidaysTableComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ HolidaysTableComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(HolidaysTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
