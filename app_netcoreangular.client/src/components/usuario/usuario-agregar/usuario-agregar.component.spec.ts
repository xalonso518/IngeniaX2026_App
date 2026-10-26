import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UsuarioAgregarComponent } from './usuario-agregar.component';

describe('UsuarioAgregarComponent', () => {
  let component: UsuarioAgregarComponent;
  let fixture: ComponentFixture<UsuarioAgregarComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UsuarioAgregarComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(UsuarioAgregarComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
