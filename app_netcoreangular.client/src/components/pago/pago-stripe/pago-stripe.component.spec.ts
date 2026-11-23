import { ComponentFixture, TestBed } from '@angular/core/testing';
import { PagoStripeComponent } from './pago-stripe.component';

describe('PagoStripeComponent', () => {
  let component: PagoStripeComponent;
  let fixture: ComponentFixture<PagoStripeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PagoStripeComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(PagoStripeComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
