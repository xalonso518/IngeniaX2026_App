import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UsuarioBuscarComponent } from './usuario-buscar.component';

describe('UsuarioBuscarComponent', () => {
  let component: UsuarioBuscarComponent;
  let fixture: ComponentFixture<UsuarioBuscarComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UsuarioBuscarComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(UsuarioBuscarComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
