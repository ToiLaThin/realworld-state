import { createReducer, on } from '@ngrx/store'
import { IAuthState } from './authState.interface'
import { loginActions, logoutActions, settingsActions } from './auth.actions'
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
      // Clear previous errors
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
    settingsActions.updateSettings,
    (state): IAuthState => ({
      ...state,
      validationErrors: null,
    })
  ),
  on(
    settingsActions.updateSettingsSuccess,
    (state, action): IAuthState => ({
      ...state,
      currentUser: action.updatedUser,
    })
  ),
  on(
    settingsActions.updateSettingsFailure,
    (state, action): IAuthState => ({
      ...state,
      validationErrors: action.errors,
    })
  ),
  on(
    logoutActions.logout,
    (state): IAuthState => ({
      ...state,
      currentUser: null,
      isLoggedIn: false,
    })
  ),
  on(logoutActions.logoutSuccess, state => ({
    ...state,
  }))
)
