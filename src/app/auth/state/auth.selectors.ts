import { createSelector } from '@ngrx/store'
import { IAuthState } from '../types/authState.interface'
import { authFeatureKey } from './auth.reducers'
import { IErrors } from '../../core/ui-models/errors.interface'

//return a slice of the state (auth feature): IAuthState
export const selectAuthFeature = (state: { [authFeatureKey]: IAuthState }) => state[authFeatureKey]
export const selectorIsSubmittingLoginRequest = createSelector(
  selectAuthFeature,
  authState => authState.isSubmittingLoginRequest
)
export const selectorCurrentUser = createSelector(
  selectAuthFeature,
  authState => authState.currentUser
)
export const selectorIsLoggedIn = createSelector(
  selectAuthFeature,
  authState => authState.isLoggedIn
)
export const selectorHaveValidationErrors = createSelector(
    selectAuthFeature,
    authState => !!authState.validationErrors
)

export const selectorLoginValidationErrors = createSelector(
  selectAuthFeature,
  authState => (authState.validationErrors as IErrors).errors
)
