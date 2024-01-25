import { Injectable } from '@angular/core'
import { Actions, createEffect, ofType } from '@ngrx/effects'
import { TagService } from '../../core/services/tag.service'
import { editorActions, homeActions, tagActions } from './home.actions'
import { catchError, map, of, switchMap, tap } from 'rxjs'
import { ArticleService } from '../../core/services/article.service'
import { Store } from '@ngrx/store'
import { selectorFeedType, selectorFilterBy } from './home.selectors'
import { homeFeatureKey } from './home.reducers'
import { IHomeState } from './homeState.interface'
import { IFilter } from '../../core/models/filter.interface'
import { FeedType } from '../../core/ui-models/feed-types.enum'

@Injectable()
export class HomeEffects {
  constructor(
    private store: Store,
    private actions$: Actions,
    private tagService: TagService,
    private articleService: ArticleService
  ) {}
  tagLoadEffects$ = createEffect(() =>
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

  //get state data from store in an effect, and pass it to service
  homeArticlesReloadEffects$ = createEffect(() =>
    this.actions$.pipe(
      ofType(homeActions.reloadArticles),
      switchMap(() => {
        let filterValue: IFilter | null = null
        let feedTypeValue: FeedType | null = null
        //subscribe here, just pipe & tap (to get the value), but subscribe to take the value
        let filterBySubscription = this.store
          .select(state => selectorFilterBy(state as { [homeFeatureKey]: IHomeState }))
          .pipe(tap(filter => (filterValue = filter)))
          .subscribe()
        let feedTypeSubscription = this.store
          .select(state => selectorFeedType(state as { [homeFeatureKey]: IHomeState }))
          .pipe(tap(feedType => (feedTypeValue = feedType)))
          .subscribe()
        console.log('filterValue', filterValue as unknown as IFilter)
        console.log('feedTypeValue', feedTypeValue as unknown as FeedType)
        //we need to unsubscribe to avoid memory leak
        filterBySubscription.unsubscribe()
        feedTypeSubscription.unsubscribe()
        return this.articleService
          .listArticles(filterValue as unknown as IFilter, feedTypeValue as unknown as FeedType)
          .pipe(
            map(response => homeActions.loadArticlesSuccess({ loadedArticles: response.articles, totalArticlesCount: response.articlesCount })),
            catchError(errors => of(homeActions.loadArticlesFailure({ errors })))
          )
      })
    )
  )

  //all five actions will trigger reloadArticles action (simply put event trigger another event not http service call)
  homeFilterByOrPageOrFeedChangeEffects$ = createEffect(() =>
    this.actions$.pipe(
      ofType(
        homeActions.filterByTagChanged,
        homeActions.filterByAuthorChanged,
        homeActions.filterByFavoritedChanged,
        homeActions.choosePage,
        homeActions.setFeedType
      ),
      map(() => homeActions.reloadArticles())
    )
  )
}
