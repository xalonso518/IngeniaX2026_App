import { HttpClientModule } from '@angular/common/http';
import { NgModule, provideBrowserGlobalErrorListeners } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing-module';
import { App } from './app';
import { SingleLayoutComponent } from '../containers/single-layout/single-layout.component';
import { LoadingService } from '../components/shared/loading/loading.service';
import { AuthGuard } from '../utils/services/auth-guard.service';
import { SessionService } from '../utils/services/session.service';
import { ErrorValidationService } from '../utils/services/error-validation.service';
import { ApiClientService } from '../utils/services/api-client.service';
import { HandleErrorService } from '../utils/services/handle-error.service';
import { ConfigService } from '../utils/services/config.service';

@NgModule({
  declarations: [
    App,
    SingleLayoutComponent
  ],
  imports: [
    BrowserModule, HttpClientModule,
    AppRoutingModule
  ],
  providers: [
    ConfigService,
    HandleErrorService,
    ApiClientService,
    ErrorValidationService,
    SessionService,
    AuthGuard,
    LoadingService,
    provideBrowserGlobalErrorListeners(),
  ],
  bootstrap: [App]
})
export class AppModule { }
