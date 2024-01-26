import { Injectable } from '@angular/core'
import { ProfileService } from '../../core/services/profile.service'
import { createEffect, ofType } from '@ngrx/effects'
import { Actions } from '@ngrx/effects'
import { profileActions } from './profile.actions'
import { catchError, map, of, switchMap } from 'rxjs'
import { UserService } from '../../core/services/user.service'

@Injectable({
  providedIn: 'root',
})
export class ProfileEffect {
  constructor(private actions$: Actions, private profileService: ProfileService, private userService: UserService) {}

  profileEffect$ = createEffect(() =>
    this.actions$.pipe(
      ofType(profileActions.profileOfUserSelected),
      switchMap(action => {
        return this.profileService.getProfileOfUsername(action.username).pipe(
          map(profile => profileActions.getProfileSuccess({ returnedProfile: profile })),
          catchError(errors => of(profileActions.getProfileFailure({ errors })))
        )
      })
    )
  )

  followProfileEffect$ = createEffect(() =>
    this.actions$.pipe(
      ofType(profileActions.followProfile),
      switchMap(action => {
        return this.userService.followUser(action.username).pipe(
          map(profile => profileActions.followOrUnfollowProfileSuccess({ returnedProfile: profile })),
          catchError(errors => of(profileActions.followOrUnfollowProfileFailure({ errors })))
        )
      })
    )
  )

  unfollowProfileEffect$ = createEffect(() =>
    this.actions$.pipe(
      ofType(profileActions.unfollowProfile),
      switchMap(action => {
        return this.userService.unfollowUser(action.username).pipe(
          map(profile => profileActions.followOrUnfollowProfileSuccess({ returnedProfile: profile })),
          catchError(errors => of(profileActions.followOrUnfollowProfileFailure({ errors })))
        )
      })
    )
  )
}
