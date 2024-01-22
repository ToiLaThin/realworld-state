import { createReducer, on } from '@ngrx/store'
import { IAuthState } from '../types/authState.interface'
import { loginActions, logoutActions } from './auth.actions'
import { IUser } from './../../core/models/user.interface'
export const initialAuthState: IAuthState = {
  isSubmittingLoginRequest: false,
  currentUser: null,
  isLoggedIn: null,
  validationErrors: null,
}

export const authFeatureKey = 'auth'
export const authReducer = createReducer(
  initialAuthState,
  on(
    loginActions.login,
    (state): IAuthState => ({
      ...state,
      isSubmittingLoginRequest: true,
      validationErrors: null,
    })
  ),
  on(loginActions.loginSuccess, (state, action) => ({
    ...state,
    isSubmittingLoginRequest: false,
    currentUser: action.returnedUser,
    isLoggedIn: true,
  })),
  on(loginActions.loginFailure, (state, action) => ({
    ...state,
    isSubmittingLoginRequest: false,
    validationErrors: action.errors,
  })),
  on(
    logoutActions.logout,
    (state): IAuthState => ({
      ...state,
      isSubmittingLoginRequest: false,
      currentUser: null,
      isLoggedIn: false,
      validationErrors: null,
    })
  )
)
