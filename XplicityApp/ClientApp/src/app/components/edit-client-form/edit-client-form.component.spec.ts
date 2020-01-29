import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EditClientFormComponent } from './edit-client-form.component';

describe('EditClientFormComponent', () => {
  let component: EditClientFormComponent;
  let fixture: ComponentFixture<EditClientFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EditClientFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditClientFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
