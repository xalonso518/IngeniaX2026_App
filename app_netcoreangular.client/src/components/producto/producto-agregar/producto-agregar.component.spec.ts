import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ProductoAgregarComponent } from './producto-agregar.component';

describe('ProductoAgregarComponent', () => {
  let component: ProductoAgregarComponent;
  let fixture: ComponentFixture<ProductoAgregarComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProductoAgregarComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(ProductoAgregarComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
