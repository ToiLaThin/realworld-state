import { NgModule } from '@angular/core'
import { LoginComponent } from './components/login.component'
import { RegisterComponent } from './components/register.component'
import { StoreModule } from '@ngrx/store'
import { authFeatureKey, authReducer } from './state/auth.reducers'
import { RouterModule } from '@angular/router'
import { authRoutes } from './auth.routes'
import { SharedModule } from '../shared/shared.module'
import { EffectsModule } from '@ngrx/effects'
import { AuthEffect } from './state/auth.effects'
import { UserService } from './services/user.service'

@NgModule({
  declarations: [LoginComponent, RegisterComponent],
  imports: [
    StoreModule.forFeature(authFeatureKey, authReducer),
    EffectsModule.forFeature([AuthEffect]),
    RouterModule.forChild(authRoutes),
    SharedModule,
  ],
  providers: [UserService],
  exports: [],
})
export class AuthModule {}
