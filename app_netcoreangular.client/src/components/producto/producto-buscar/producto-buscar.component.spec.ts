import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ProductoBuscarComponent } from './producto-buscar.component';

describe('ProductoBuscarComponent', () => {
  let component: ProductoBuscarComponent;
  let fixture: ComponentFixture<ProductoBuscarComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProductoBuscarComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(ProductoBuscarComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
