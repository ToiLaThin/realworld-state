import { Component, OnInit } from '@angular/core'
import { ILoginRequest } from '../types/loginRequest.interface'
import { Store } from '@ngrx/store'
import { loginActions } from '../../state/auth/auth.actions'
import { Observable } from 'rxjs'
import { selectorHaveValidationErrors, selectorLoginValidationErrors } from '../../state/auth/auth.selectors'
import { authFeatureKey } from '../../state/auth/auth.reducers'
import { IAuthState } from '../../state/auth/authState.interface'
import { FormBuilder, Validators } from '@angular/forms'
@Component({
  selector: 'rw-login',
  templateUrl: './login.component.html',
})
export class LoginComponent implements OnInit {
  haveLoginErrors$!: Observable<boolean>
  loginErrors$!: Observable<object>
  isFormSubmitting$!: Observable<boolean>
  loginForm = this.fb.group({
    email: ['', Validators.required],
    password: ['', Validators.required],
  })

  constructor(private store: Store, private fb: FormBuilder) {}

  ngOnInit(): void {
    this.haveLoginErrors$ = this.store.select(state =>
      selectorHaveValidationErrors(state as { [authFeatureKey]: IAuthState })
    )
    this.loginErrors$ = this.store.select(
      state => selectorLoginValidationErrors(state as { [authFeatureKey]: IAuthState })
    )
    this.isFormSubmitting$ = this.store.select(
      state => (state as { [authFeatureKey]: IAuthState })[authFeatureKey].isSubmittingLoginRequest
    )
  }

  login() {
    const loginRequest: ILoginRequest = {
      email: this.loginForm.value.email!,
      password: this.loginForm.value.password!,
    }
    this.store.dispatch(loginActions.login({ loginRequest }))
  }
}
