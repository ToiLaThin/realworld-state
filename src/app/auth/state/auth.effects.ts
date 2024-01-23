import { Actions, createEffect, ofType } from '@ngrx/effects'
import { UserService } from '../services/user.service'
import { loginActions } from './auth.actions'
import { catchError, map, of, switchMap } from 'rxjs'
import { Injectable } from '@angular/core'

@Injectable()
export class AuthEffect {
  constructor(private actions$: Actions, private userService: UserService) {}

  loginEffect$ = createEffect(() =>
    this.actions$.pipe(
      ofType(loginActions.login),
      switchMap(action => {
        return this.userService.login(action.loginRequest).pipe(
          map(user => loginActions.loginSuccess({ returnedUser: user })),
          catchError(errors => of(loginActions.loginFailure({ errors })))
        )
      })
    )
  )
}
