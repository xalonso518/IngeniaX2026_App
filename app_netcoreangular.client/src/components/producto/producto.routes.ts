import { Routes } from '@angular/router';
import { ProductoBuscarComponent } from './producto-buscar/producto-buscar.component';
import { ProductoAgregarComponent } from './producto-agregar/producto-agregar.component';
import { ProductoEditarComponent } from './producto-editar/producto-editar.component';

export const PRODUCTO_ROUTES: Routes = [
  { path: '', component: ProductoBuscarComponent },
  { path: 'agregar', component: ProductoAgregarComponent },
  { path: 'editar/:id', component: ProductoEditarComponent },
];
