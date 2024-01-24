import { Actions, createEffect, ofType } from '@ngrx/effects'
import { UserService } from '../../core/services/user.service'
import { loginActions, logoutActions, settingsActions } from './auth.actions'
import { catchError, map, of, switchMap, tap } from 'rxjs'
import { Injectable } from '@angular/core'
import { Router } from '@angular/router'
import { SettingsService } from '../../core/services/settings.service'
import { JwtService } from '../../shared/services/jwt.service'

@Injectable()
export class AuthEffect {
  constructor(
    private actions$: Actions,
    private userService: UserService,   
    private settingsService: SettingsService, 
    private jwtService: JwtService,
    private router: Router
  ) {}

  loginEffect$ = createEffect(() =>
    this.actions$.pipe(
      ofType(loginActions.login),
      switchMap(action => {
        return this.userService.login(action.loginRequest).pipe(
          map(user => loginActions.loginSuccess({ returnedUser: user })),          
          tap(_ => this.router.navigateByUrl('/')),
          catchError(errors => of(loginActions.loginFailure({ errors })))
        )
      })
    )
  )

  settingsEffect$ = createEffect(() =>
    this.actions$.pipe(
      ofType(settingsActions.updateSettings),
      switchMap(action => {
        return this.settingsService.updateSettings(action.settingsRequest).pipe(
          map(user => settingsActions.updateSettingsSuccess({ updatedUser: user })),
          catchError(errors => of(settingsActions.updateSettingsFailure({ errors })))
        )
      })
    )
  )

  logoutEffect$ = createEffect(() =>
    this.actions$.pipe(
      ofType(logoutActions.logout),
      //side effect on observable
      tap(_ => {
        this.jwtService.destroyToken()
        this.router.navigateByUrl('/')
      }),
      switchMap(_ => of(logoutActions.logoutSuccess()))
    )
  )
}
