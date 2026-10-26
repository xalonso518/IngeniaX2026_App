import { MatMenuModule } from '@angular/material/menu';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatDialogTitle, MatDialogContent, MatDialogActions, MatDialogClose } from '@angular/material/dialog';

// Importacion de modulos para pantallas de busqueda
export const MATERIAL_MODULES_BUSCAR = [
  MatButtonModule,
  MatMenuModule,
  MatIconModule,
  MatButtonToggleModule,
  MatPaginatorModule,
  MatProgressSpinnerModule
];

export const MATERIAL_MODULES_MODAL = [
  MatDialogTitle,
  MatDialogContent,
  MatDialogActions,
  MatDialogClose
];
