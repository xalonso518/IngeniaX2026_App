import { Routes } from '@angular/router';
import { PagoStripeComponent } from './pago-stripe/pago-stripe.component';
import { PagoCancelComponent } from './pago-cancel/pago-cancel.component';
import { PagoSuccessComponent } from './pago-success/pago-success.component';

export const PAGO_ROUTES: Routes = [
  { path: '', component: PagoStripeComponent },
  { path: 'success', component: PagoSuccessComponent },
  { path: 'cancel', component: PagoCancelComponent },
];
