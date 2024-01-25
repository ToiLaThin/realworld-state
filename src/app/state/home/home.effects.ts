import { Injectable } from '@angular/core'
import { Actions, createEffect, ofType } from '@ngrx/effects'
import { TagService } from '../../core/services/tag.service'
import { editorActions, tagActions } from './home.actions'
import { catchError, map, of, switchMap } from 'rxjs'
import { ArticleService } from '../../core/services/article.service'

@Injectable()
export class HomeEffects {
  constructor(
    private actions$: Actions,
    private tagService: TagService,
    private articleService: ArticleService
  ) {}
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

  //3 effects but better readability
  editorFormSubmmitedEffects$ = createEffect(() =>
    this.actions$.pipe(
      ofType(editorActions.submitEditorForm),
      switchMap(action => {
        if (
          action.article.slug === null ||
          action.article.slug === undefined ||
          action.article.slug === ''
        ) {
          return of(editorActions.createNewArticle({ article: action.article }))
        }
        return of(editorActions.updateArticle({ article: action.article }))
      })
    )
  )

  editorCreateArticleEffects$ = createEffect(() =>
    this.actions$.pipe(
      ofType(editorActions.createNewArticle),
      switchMap(action => {
        return this.articleService.createArticle(action.article).pipe(
          map(article => editorActions.submitEditorFormSuccess({ submittedArticle: article })),
          catchError(errors => of(editorActions.submitEditorFormFailure({ errors: errors })))
        )
      })
    )
  )

  editorUpdateArticleEffects$ = createEffect(() =>
    this.actions$.pipe(
      ofType(editorActions.updateArticle),
      switchMap(action => {
        return this.articleService.updateArticle(action.article).pipe(
          map(article => editorActions.submitEditorFormSuccess({ submittedArticle: article })),
          catchError(errors => of(editorActions.submitEditorFormFailure({ errors: errors })))
        )
      })
    )
  )
}
