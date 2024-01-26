import { createFeatureSelector, createSelector } from '@ngrx/store'
import { profileFeatureKey } from './profile.reducers'
import { IProfileState } from './profileState,interface'
import { authFeatureKey } from '../auth/auth.reducers'
import { IAuthState } from './../auth/authState.interface'

export const selectProfileFeature = (state: { [profileFeatureKey]: IProfileState }) =>
  state[profileFeatureKey]
export const selectorIsLoadingProfile = createSelector(
  selectProfileFeature,
  profileState => profileState.isLoadingProfile
)

export const selectorViewingProfile = createSelector(
  selectProfileFeature,
  profileState => profileState.viewingProfile
)

//local select feature state: from multiple state, we can aggregate to get the state we want
const selectAuthFeatureLocal = createFeatureSelector<IAuthState>(authFeatureKey)
const selectProfileFeatureLocal = createFeatureSelector<IProfileState>(profileFeatureKey)
export const selectorIsThisProfileOfCurrentUser = createSelector(
  selectAuthFeatureLocal,
  selectProfileFeatureLocal,
  (authState, profileState) => {
    return authState.currentUser?.username === profileState.viewingProfile?.username
  }
)

export const selectorIsFollowOrUnfollowProfileInProgress = createSelector(
  selectProfileFeature,
  profileState => profileState.isFollowOrUnfollowProfileInProgress
)
