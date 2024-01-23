import { createActionGroup, emptyProps, props } from '@ngrx/store'
import { IArticle } from '../../core/models/article.interface'
import { ILoginRequest } from '../types/loginRequest.interface'
import { IUser } from '../../core/models/user.interface'
import { IErrors } from '../../core/ui-models/errors.interface'

//actions are the events that are dispatched to the store
//to trigger reducers (which are pure functions) to update the state
export const loginActions = createActionGroup({
  source: 'Auth Feature',
  events: {
    'Login': props<{ loginRequest: ILoginRequest }>(),
    'Login Success': props<{ returnedUser: IUser }>(),
    'Login Failure': props<{ errors: IErrors }>(),
  },
})

//still have the same namespace, just different variable name
export const logoutActions = createActionGroup({
  source: 'Auth Feature',
  events: {
    Logout: emptyProps(),
  },
})
