import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from '../components/auth/login/login.component';
import { SingleLayoutComponent } from '../containers/single-layout/single-layout.component';
import { MainLayoutComponent } from '../containers/main-layout/main-layout.component';

const routes: Routes = [
  { path: '', redirectTo: 'auth/login', pathMatch: 'full' },
  //lazy loading - Técnica para cargar según demanda
  {
    path: 'auth',
    component: SingleLayoutComponent,
    loadChildren: () => import('../components/auth/auth.routes').then(r => r.AUTH_ROUTES),
  },

  // Rutas de usuarios
  {
    path: 'usuario',
    component: MainLayoutComponent,
    loadChildren: () => import('../components/usuario/usuario.routes').then(r => r.USUARIO_ROUTES),
  },

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
