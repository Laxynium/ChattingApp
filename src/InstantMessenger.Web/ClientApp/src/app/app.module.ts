import {BrowserModule} from '@angular/platform-browser';
import {NgModule} from '@angular/core';
import {HttpClientModule, HTTP_INTERCEPTORS} from '@angular/common/http';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';
import {CommonModule} from '@angular/common';
import {StoreModule} from '@ngrx/store';

import {AppRoutingModule} from './app-routing.module';
import {AppComponent} from './app.component';
import {IdentityModule} from './identity/identity.module';
import {StoreDevtoolsModule} from '@ngrx/store-devtools';
import {environment} from '../environments/environment';
import {EffectsModule} from '@ngrx/effects';
import {ToastService} from './shared/toasts/toast.service';
import {ToastsContainer} from 'src/app/shared/toasts/toasts.container.component';
import {AuthInterceptor} from './identity/services/authentication.interceptor.service';
import {PersistanceService} from './shared/services/persistance.service';
import {HomeModule} from './home/home.module';
import {RequestFailedEffect} from 'src/app/shared/store/api-request.error';
import {
  FaIconLibrary,
  FontAwesomeModule,
} from '@fortawesome/angular-fontawesome';
import {
  faBan,
  faCog,
  faCogs,
  faComments,
  faEllipsisH,
  faPlus,
  faSignInAlt,
  faSignOutAlt,
  faSpinner,
  faSync,
  faUserCheck,
  faUserMinus,
  faUsers,
  faUserTimes,
  faInfinity,
  faTrashAlt,
  faCrown,
  faPaperPlane,
  faTimesCircle,
  faCheckCircle,
  faSlash,
  faChevronUp,
  faChevronDown,
} from '@fortawesome/free-solid-svg-icons';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';

@NgModule({
  declarations: [AppComponent, ToastsContainer],
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    CommonModule,
    NgbModule,
    FormsModule,
    ReactiveFormsModule,
    FontAwesomeModule,
    StoreModule.forRoot([]),
    IdentityModule,
    HomeModule,
    StoreDevtoolsModule.instrument({
      maxAge: 25,
      logOnly: environment.production,
    }),
    EffectsModule.forRoot([RequestFailedEffect]),
    FontAwesomeModule,
  ],
  providers: [
    ToastService,
    PersistanceService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true,
    },
  ],
  bootstrap: [AppComponent],
})
export class AppModule {
  constructor(private library: FaIconLibrary) {
    library.addIcons(
      faUsers,
      faComments,
      faUserMinus,
      faBan,
      faUserCheck,
      faUserTimes,
      faCogs,
      faCog,
      faSpinner,
      faSync,
      faEllipsisH,
      faSignInAlt,
      faSignOutAlt,
      faPlus,
      faInfinity,
      faTrashAlt,
      faCrown,
      faPaperPlane,
      faTimesCircle,
      faCheckCircle,
      faSlash,
      faChevronUp,
      faChevronDown
    );
  }
}
