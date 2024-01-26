import { createReducer, on } from '@ngrx/store'
import { IProfileState } from './profileState,interface'
import { profileActions } from './profile.actions'

export const initialProfileState: IProfileState = {
  isLoadingProfile: false,
  viewingProfile: null,
}

export const profileFeatureKey = 'profile'
export const profileReducer = createReducer(
  initialProfileState,
  on(
    profileActions.profileOfUserSelected,
    (state): IProfileState => ({
      ...state,
      isLoadingProfile: true,
    })
  ),
  on(
    profileActions.getProfileSuccess,
    (state, action) => ({
        ...state,
        isLoadingProfile: false,
        viewingProfile: action.returnedProfile,
    })
  ),
    on(
        profileActions.getProfileFailure,
        (state, action) => ({
            ...state,
            isLoadingProfile: false,
        })
    ),

)
