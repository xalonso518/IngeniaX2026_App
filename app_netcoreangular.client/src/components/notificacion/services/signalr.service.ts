import { Injectable, signal } from '@angular/core';
import * as signalR from '@microsoft/signalr';

@Injectable({ providedIn: 'root' })
export class SignalRService {
  private hubConnection!: signalR.HubConnection;
  public latestPrice: number = 0;
  //notificacion = signal("");
  public notificaciones = signal<string[]>([]);

  // CONSTRUCTOR
  constructor() {
  }

  // Initialize the SignalR connection
  async connect(token: string, usuarioId: string = 'demo-user'): Promise<void> {

    // Validamos conexxion actual
    if (this.hubConnection != undefined) {
      if (this.hubConnection.state === signalR.HubConnectionState.Connected) {
        //console.log("SignalR is connected!");
        return;
      }
    }

    // Iniciamos conexion
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`http://localhost:5044/api/hub?userId=${encodeURIComponent(usuarioId)}`, { accessTokenFactory: () => token })
      //.withUrl('https://localhost:7009/api/hub', { accessTokenFactory: () => token })
      //.withUrl(`http://localhost:5000/api/hub?userId=${encodeURIComponent(userId)}`)
      .withAutomaticReconnect([0, 2000, 5000, 10000, 30000]) // Retry intervals
      //.configureLogging(signalR.LogLevel.Information)
      .build();

    // Registramos handler para recibir notificaciones
    this.hubConnection.on('ReceiveNotification', (notification: string) => {
      console.log(notification)
    });

    this.hubConnection.on('ReceiveNotificationUser', (notification: string) => {
      this.notificaciones.update(currentItems => [...currentItems, notification]);
      //this.notificacion.set(notification);
      console.log(notification);
      // Posiblemnte mostremos mensaje notificacion
    });


    this.hubConnection.on('ReceiveNotificationAll', (notification: string) => {
      console.log(notification)
    });


    // Handle para reconexiones
    this.hubConnection.onreconnecting((error) => {
      console.log('Reconnecting to SignalR...', error);
    });

    this.hubConnection.onreconnected(async (connectionId) => {
      console.log('Reconnected to SignalR:', connectionId);
    });

    try {
      await this.hubConnection.start();
      //console.log('SignalR connected');
    } catch (err) {
      console.error('SignalR connection failed:', err);
      // Retry after 5 seconds
      await new Promise((resolve) => setTimeout(resolve, 5000));
      return this.connect(token, usuarioId);
    }
  }

  // startConnection(token: string, usuarioId:string): void {
  //   //var usuarioId = "lgv.alonso@gmail.com"
  //     //.withUrl('https://localhost:7009/api/priceHub?userId=' + usuarioId, { accessTokenFactory: () => token })
  //   this.hubConnection = new signalR.HubConnectionBuilder()
  //     .withUrl('https://localhost:7009/api/priceHub', { accessTokenFactory: () => token })
  //     //.withAutomaticReconnect([0, 2000, 5000, 10000, 30000]) // Retry intervals
  //     .build();

  //   this.hubConnection
  //     .start()
  //     .then((r) => {
  //       console.log('SignalR connection started');
  //       //console.log(r);
  //       })
  //     .catch(err => console.error('Error connecting to SignalR', err));

  //   this.hubConnection.on('ReceivePriceUpdate', (symbol, price) => {
  //     this.latestPrice = price;
  //     console.log(`${symbol} => ${price}`);
  //   });

    
  // }
}
