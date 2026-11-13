import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NotificacionAlertaComponent } from './notificacion-alerta.component';

describe('NotificacionAlertaComponent', () => {
  let component: NotificacionAlertaComponent;
  let fixture: ComponentFixture<NotificacionAlertaComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [NotificacionAlertaComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(NotificacionAlertaComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
