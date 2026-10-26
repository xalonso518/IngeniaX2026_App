import { Component, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MainNavBarComponent } from '../main-nav-bar/main-nav-bar.component';
import { MainSideBarComponent } from '../main-side-bar/main-side-bar.component';
import { RouterOutlet } from '@angular/router';
import { LoadingService } from '../../components/shared/loading/loading.service';
import { LoadingComponent } from '../../components/shared/loading/loading.component';

@Component({
  selector: 'app-main-layout',
  imports: [RouterLink, RouterOutlet, MainNavBarComponent, MainSideBarComponent, MatSidenavModule, MatToolbarModule, LoadingComponent],
  templateUrl: './main-layout.component.html',
  styleUrl: './main-layout.component.css',
})
export class MainLayoutComponent {
  opened = signal(true); // Sidebar true = grande, false = reducido de tamaño
  side_true = signal(true); // Sidebar mostrado(default) = true, oculto(mobil)

  /** inicio-layout ctor */
  constructor(
    public loadingService: LoadingService,
  ) {
  }

  ngOnInit(): void {
    this.loadingService.hide();
  }
}
