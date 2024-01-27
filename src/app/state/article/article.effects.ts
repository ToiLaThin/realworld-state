import { Actions, createEffect, ofType } from '@ngrx/effects'
import { articleActions } from './article.actions'
import { catchError, map, of, switchMap, tap } from 'rxjs'
import { ArticleService } from '../../core/services/article.service'
import { Injectable } from '@angular/core'
import { homeActions } from '../home/home.actions'
import { Router } from '@angular/router'
import { CommentService } from '../../core/services/comment.service'

@Injectable({ providedIn: 'root' })
export class ArticleEffect {
  constructor(
    private actions$: Actions,
    private articleService: ArticleService,
    private commentService: CommentService,
    private router: Router
  ) {}

  articleEffects$ = createEffect(() =>
    this.actions$.pipe(
      ofType(articleActions.selectArticle),
      map(action => {
        return articleActions.loadArticle({
          selectedArticleSlug: action.selectedArticleSlug,
          displayIsLoading: action.displayIsLoading,
        })
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

  afterFavoriteOrUnfavoriteArticleReloadSelectedArticleEffects$ = createEffect(() =>
    this.actions$.pipe(
      ofType(homeActions.favoriteOrUnfavoriteArticleSuccess),
      map(action =>
        articleActions.loadArticle({
          selectedArticleSlug: action.returnArticle.slug as string,
          displayIsLoading: false,
        })
      )
    )
  )

  deleteArticleEffects$ = createEffect(() =>
    this.actions$.pipe(
      ofType(articleActions.deleteArticle),
      tap(() => this.router.navigateByUrl('/')),
      switchMap(action => {
        return this.articleService.deleteArticle(action.articleSlug).pipe(
          map(_ => articleActions.deleteArticleSuccess()),
          catchError(errors => of(articleActions.deleteArticleFailure({ errors })))
        )
      })
    )
  )

  reloadArticleCommentsEffects$ = createEffect(() =>
    this.actions$.pipe(
      ofType(articleActions.reloadArticleComments),
      switchMap(action => {
        return this.commentService.getArticleComments(action.articleSlug).pipe(
          map(returnedComments =>
            articleActions.reloadArticleCommentsSuccess({ returnedComments: returnedComments })
          ),
          catchError(errors => of(articleActions.reloadArticleCommentsFailure({ errors })))
        )
      })
    )
  )

  addCommentToArticleEffects$ = createEffect(() =>
    this.actions$.pipe(
      ofType(articleActions.addCommentToArticle),
      switchMap(action => {
        let articleSlug = action.articleSlug
        return this.commentService.addCommentToArticle(action.articleSlug, action.commentBody).pipe(
          map(returnedComment => articleActions.reloadArticleComments({ articleSlug })),
          catchError(errors => of(articleActions.reloadArticleCommentsFailure({ errors })))
        )
      })
    )
  )

  deleteCommentFromArticleEffects$ = createEffect(() =>
    this.actions$.pipe(
      ofType(articleActions.deleteCommentFromArticle),
      switchMap(action => {
        let articleSlug = action.articleSlug
        return this.commentService.deleteCommentFromArticle(action.articleSlug, action.commentId).pipe(
          map(_ => articleActions.reloadArticleComments({ articleSlug })),
          catchError(errors => of(articleActions.reloadArticleCommentsFailure({ errors })))
        )
      })
    )
  )
}
