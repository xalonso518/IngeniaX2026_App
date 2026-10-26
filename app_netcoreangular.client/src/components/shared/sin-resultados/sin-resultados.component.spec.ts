import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SinResultadosComponent } from './sin-resultados.component';

describe('SinResultadosComponent', () => {
  let component: SinResultadosComponent;
  let fixture: ComponentFixture<SinResultadosComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SinResultadosComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(SinResultadosComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
