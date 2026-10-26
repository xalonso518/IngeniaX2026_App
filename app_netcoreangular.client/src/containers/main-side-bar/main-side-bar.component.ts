import { Component, model } from '@angular/core';
import { SessionService } from '../../utils/services/session.service';
import { MatExpansionModule } from '@angular/material/expansion';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { MatMenuModule } from '@angular/material/menu';
import { MatButtonModule } from '@angular/material/button';
import { ConstantsSpaRoutes } from '../../utils/constants/ConstantsSpaRoutes';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-main-side-bar',
  imports: [MatExpansionModule, RouterLink, MatButtonModule, MatMenuModule, RouterLink, RouterLinkActive, MatIconModule],
  templateUrl: './main-side-bar.component.html',
  styleUrl: './main-side-bar.component.css',
})
export class MainSideBarComponent {
  opened = model(false); // Variable open del padre
  SpaRoutes = ConstantsSpaRoutes;

  /** SidebarPanel ctor */
  constructor(
    public sessionService: SessionService) {
  }

  // Funcion inicial
  ngOnInit(): void {
    // Para validar permisos
  }
}
