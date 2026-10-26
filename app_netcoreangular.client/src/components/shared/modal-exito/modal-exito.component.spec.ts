import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ModalExitoComponent } from './modal-exito.component';

describe('ModalExitoComponent', () => {
  let component: ModalExitoComponent;
  let fixture: ComponentFixture<ModalExitoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ModalExitoComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(ModalExitoComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
