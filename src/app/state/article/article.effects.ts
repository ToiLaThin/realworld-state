import { Actions, createEffect, ofType } from '@ngrx/effects'
import { articleActions } from './article.actions'
import { catchError, map, of, switchMap } from 'rxjs'
import { ArticleService } from '../../core/services/article.service'
import { Injectable } from '@angular/core'

@Injectable({ providedIn: 'root' })
export class ArticleEffect {
  constructor(private actions$: Actions, private articleService: ArticleService) {}

  articleEffects$ = createEffect(() =>
    this.actions$.pipe(
      ofType(articleActions.selectArticle),
      map(action => {
        return articleActions.loadArticle({ selectedArticleSlug: action.selectedArticleSlug })
      })
    )
  )

  articleLoadCompletedEffects$ = createEffect(() =>
    this.actions$.pipe(
      ofType(articleActions.loadArticle),
      switchMap(action =>
        this.articleService.getArticle(action.selectedArticleSlug).pipe(
          map(returnedArticle => articleActions.loadArticleSuccess({ returnedArticle })),
          catchError(errors => of(articleActions.loadArticleFailure({ errors })))
        )
      )
    )
  )
}
