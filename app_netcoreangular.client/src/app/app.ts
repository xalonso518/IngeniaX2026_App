import { HttpClient } from '@angular/common/http';
import { Component, OnInit, signal } from '@angular/core';

interface WeatherForecast {
  date: string;
  temperatureC: number;
  temperatureF: number;
  summary: string;
}

@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  standalone: false,
  styleUrl: './app.css'
})
export class App implements OnInit { 
  public forecasts = signal<WeatherForecast[]>([]);

  constructor(private http: HttpClient) {}

  ngOnInit() {
    this.getForecasts();
  }

  getForecasts() {
    var url = 'http://localhost:5044/weatherforecast';
    this.http.get<WeatherForecast[]>(url).subscribe(
      (result) => {
        this.forecasts.set(result);
      },
      (error) => {
        console.error(error);
      }
    );
  }

  protected readonly title = signal('app_netcoreangular.client');
}
