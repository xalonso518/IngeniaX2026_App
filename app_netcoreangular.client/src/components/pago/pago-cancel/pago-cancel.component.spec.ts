import { ComponentFixture, TestBed } from '@angular/core/testing';
import { PagoCancelComponent } from './pago-cancel.component';

describe('PagoCancelComponent', () => {
  let component: PagoCancelComponent;
  let fixture: ComponentFixture<PagoCancelComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PagoCancelComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(PagoCancelComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
