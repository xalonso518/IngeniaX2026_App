import { Component, signal } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { PageEvent } from '@angular/material/paginator';
import { Router } from '@angular/router';
import { MODULOS } from '../../../utils/constants/ConstantsCatalogos';
import { MESSAGES_ERROR, MESSAGES_SUCCESS } from '../../../utils/constants/ConstantsMessages';
import { ConfigService } from '../../../utils/services/config.service';
import { HandleErrorService } from '../../../utils/services/handle-error.service';
import { SessionService } from '../../../utils/services/session.service';
import { LoadingService } from '../../shared/loading/loading.service';
import { ConstantsSpaRoutes } from '../../../utils/constants/ConstantsSpaRoutes';
import { MatMenuModule } from '@angular/material/menu';
import { DatePipe } from '@angular/common';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { SignalRService } from '../services/signalr.service';

@Component({
  selector: 'app-notificacion-alerta',
  imports: [MatMenuModule, DatePipe, MatProgressSpinnerModule],
  templateUrl: './notificacion-alerta.component.html',
  styleUrl: './notificacion-alerta.component.css',
  providers: []
})
export class NotificacionAlertaComponent {

  // Catalogos
  errores = MESSAGES_ERROR;

  // Busqueda
  page = 0;
  loading = signal(false);
  notificaciones = signal<string[]>([]);
  hoy = new Date();

  // CONSTRUCTOR
  constructor(
    public sessionService: SessionService,
    public configService: ConfigService,
    public signalRService: SignalRService,
    public dialog: MatDialog,) {
  }

  // FUNCIONES
  // Inicial
  ngOnInit(): void {
    this.openConecctionSignal();
  }

  openConecctionSignal() {
    // Conectar con signalR
    var tk = localStorage.getItem('token') ?? "";
    this.signalRService.connect(tk, this.sessionService.getUsuarioUsername());
    //this.signalRService.startConnection(tk, this.sessionService.getUsuarioUsername());
  }

  onMenuOpened() {
    this.notificaciones.set(this.signalRService.notificaciones());
  }

}
