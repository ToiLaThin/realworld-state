import { NgModule } from '@angular/core'
import { LoginComponent } from './components/login.component';
import { RegisterComponent } from './components/register.component';
import { StoreModule } from '@ngrx/store';
import { authFeatureKey, authReducer } from './state/auth.reducers';

@NgModule({
  declarations: [LoginComponent, RegisterComponent],
  imports: [StoreModule.forFeature(authFeatureKey, authReducer)],
  providers: [],
  exports: [],
})
export class AuthModule {}
