import { Injectable } from '@angular/core'
import { Actions, createEffect, ofType } from '@ngrx/effects'
import { TagService } from '../services/tag.service'
import { tagActions } from './home.actions'
import { catchError, map, of, switchMap } from 'rxjs'

@Injectable()
export class HomeEffects {
  constructor(private actions$: Actions, private tagService: TagService) {}
  homeEffects$ = createEffect(() =>
    this.actions$.pipe(
      ofType(tagActions.getTags),
      switchMap(() => {
        return this.tagService.getTags().pipe(
          map(tags => tagActions.getTagsSuccess({ loadedTags: tags })),
          catchError(errors => of(tagActions.getTagsFailure({ errors })))
        )
      })
    )
  )
}
