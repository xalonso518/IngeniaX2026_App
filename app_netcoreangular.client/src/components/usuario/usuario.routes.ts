import { Routes } from '@angular/router';
import { UsuarioAgregarComponent } from './usuario-agregar/usuario-agregar.component';
import { UsuarioBuscarComponent } from './usuario-buscar/usuario-buscar.component';
import { UsuarioEditarComponent } from './usuario-editar/usuario-editar.component';

export const USUARIO_ROUTES: Routes = [
  { path: '', component: UsuarioBuscarComponent },
  { path: 'agregar', component: UsuarioAgregarComponent },
  { path: 'editar/:guid', component: UsuarioEditarComponent },
];
