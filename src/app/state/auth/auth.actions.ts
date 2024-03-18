import { createActionGroup, emptyProps, props } from '@ngrx/store'
import { ILoginRequest } from '../../auth/types/loginRequest.interface'
import { IUser } from '../../core/models/user.interface'
import { IErrors } from '../../core/ui-models/errors.interface'
import { ISettingsRequest } from '../../settings/types/settingsRequest.interface'
import { IRegisterRequest } from '../../auth/types/registerRequest.interface'

//actions are the events that are dispatched to the store
//to trigger reducers (which are pure functions) to update the state
export const loginActions = createActionGroup({
  source: 'Auth Feature Module',
  events: {
    'Login': props<{ loginRequest: ILoginRequest }>(),
    'Login Success': props<{ returnedUser: IUser }>(),
    'Login Failure': props<{ errors: IErrors }>(),
    'Register': props<{ registerRequest: IRegisterRequest }>(),
    'Register Success': props<{ returnedUser: IUser }>(),
    'Register Failure': props<{ errors: IErrors }>(),
  },
})

export const settingsActions = createActionGroup({
  source: 'Settings Feature Module',
  events: {
    'Update Settings': props<{ settingsRequest: ISettingsRequest }>(),
    'Update Settings Success': props<{ updatedUser: IUser }>(),
    'Update Settings Failure': props<{ errors: IErrors }>(),
  },
})

//still have the same namespace, just different variable name
export const logoutActions = createActionGroup({
  source: 'Settings Feature Module',
  events: {
    Logout: emptyProps(),
    'Logout Success': emptyProps(),
  },
})
