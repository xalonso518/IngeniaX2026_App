import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UsuarioEditarPasswordComponent } from './usuario-editar-password.component';

describe('UsuarioEditarPasswordComponent', () => {
  let component: UsuarioEditarPasswordComponent;
  let fixture: ComponentFixture<UsuarioEditarPasswordComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UsuarioEditarPasswordComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(UsuarioEditarPasswordComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
