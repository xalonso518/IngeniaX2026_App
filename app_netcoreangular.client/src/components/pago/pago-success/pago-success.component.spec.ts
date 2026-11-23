import { ComponentFixture, TestBed } from '@angular/core/testing';
import { PagoSuccessComponent } from './pago-success.component';

describe('PagoSuccessComponent', () => {
  let component: PagoSuccessComponent;
  let fixture: ComponentFixture<PagoSuccessComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PagoSuccessComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(PagoSuccessComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
