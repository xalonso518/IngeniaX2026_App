import { Component, signal } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { MESSAGES_ERROR, MESSAGES_SUCCESS } from '../../../utils/constants/ConstantsMessages';
import { HandleErrorService } from '../../../utils/services/handle-error.service';
import { FormControl, FormsModule } from '@angular/forms';
import { LoadingService } from '../../shared/loading/loading.service';
import { ProductoForm } from '../../../models/producto/ProductoForm';
import { PagoService } from '../services/pago.service';
import { PagoForm } from '../../../models/pago/PagoForm';

@Component({
  selector: 'app-pago-stripe',
  styleUrl: './pago-stripe.component.css',
  templateUrl: './pago-stripe.component.html',
  imports: [RouterLink, FormsModule],
  providers: [PagoService]
})
export class PagoStripeComponent {
  // PROPIEDADES
  submitted = signal(false);

  // Catalogos
  errores = MESSAGES_ERROR;

  // Form
  form = new PagoForm();

  // CONSTRUCTOR
  constructor(
    private router: Router,
    private productoService: PagoService,
    private handleError: HandleErrorService,
    private loadingService: LoadingService,
  ) {
  }

  // FUNCIONES
  // Inicial
  ngOnInit(): void {
    //var url = "https://checkout.stripe.com/c/pay/cs_test_a1TVKilcuHzmksXd19CGkiOXGNSclfGxZOFwZeInVJgp6W3MkM4jo8NNCQ#fidnandhYHdWcXxpYCc%2FJ2FgY2RwaXEnKSdicyc%2FNSknZHVsTmB8Jz8ndW5acWB2cVowNFF%2FV1JTSUZHVU5kbHdHSDdXN3BKUl1rZ2dPVVNMMWtgdXddVVRVdUpcVmxvYjBOdXZTTEs3RHNjTGcxNjRXRlMwYH9vUjBdZH1RYDVRRlRvQk9wTk1TQDU1Z3xERjVmb20nKSdjd2poVmB3c2B3Jz9xd3BgKSdnZGZuYndqcGthRmppancnPycmY2NjY2NjJyknaWR8anBxUXx1YCc%2FJ3Zsa2JpYFpscWBoJyknYGtkZ2lgVWlkZmBtamlhYHd2Jz9xd3BgeCUl";
    //window.open(url, '_blank');
  }

  // Evento submit
  submit(formAgregar: FormControl) {
    this.agregar(this.form);
  }

  // Agregar usuario api
  async agregar(form: PagoForm) {
    // Mostramos loadings
    this.submitted.set(true);
    this.loadingService.show();

    // Llenamos datos
    this.form.cantidad = 1;
    this.form.codigo = "C01";
    this.form.priceId = "price_1UJcgoLCBPKairBMJ9tsmmpA";

    // Llamamos API
    this.productoService.checkout(form)
      .subscribe({
        next: res => {
          // Validamos response
          if (res.success) {
            // Mostramos mensaje
            //this.toastr.success(MESSAGES_SUCCESS.EXITO_REGISTRO, MODULOS.USU);
            //this.router.navigate([res.payload]);
            window.open(res.payload, "_blank");

          } else this.handleError.showError(res);
        },
        error: err => {
          // Ocultamos loadings
          this.loadingService.hide();
          this.submitted.set(false);

          // Mostramos errores
          this.handleError.showError(err);
        },
        complete: () => {
          // Ocultamos loadings
          this.loadingService.hide();
          this.submitted.set(false);
        }
      });
  }
}

// 4242424242424242