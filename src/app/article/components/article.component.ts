import { Component, OnDestroy, OnInit } from '@angular/core'
import { Observable, Subscription } from 'rxjs'
import { IArticle } from '../../core/models/article.interface'
import { Store } from '@ngrx/store'
import {
  selectorArticleComments,
  selectorIsAnythingSubmittingInArticleModule,
  selectorIsCurrentUserAuthorOfSelectedArticle,
  selectorSelectedArticle,
} from '../../state/article/article.selectors'
import { articleFeatureKey } from '../../state/article/article.reducers'
import { IArticleState } from '../../state/article/articleState.interface'
import { authFeatureKey } from '../../state/auth/auth.reducers'
import { IAuthState } from '../../state/auth/authState.interface'
import { articleActions } from '../../state/article/article.actions'
import { ActivatedRoute, Router } from '@angular/router'
import { ButtonType } from '../../core/ui-models/button-types.enum'
import { profileActions } from '../../state/profile/profile.actions'
import { selectorIsLoggedIn } from '../../state/auth/auth.selectors'
import { homeActions } from '../../state/home/home.actions'
import { IComment } from '../../core/models/comment.interface'

@Component({
  selector: 'rw-article',
  templateUrl: './article.component.html',
})
export class ArticleComponent implements OnInit, OnDestroy {
  selectedArticle$!: Observable<IArticle | null>
  isLoadingArticle$!: Observable<boolean>
  isCurrUserArticleAuthor$!: Observable<boolean>
  isUserAuthenticated$!: Observable<boolean | null>

  selectedArticleSlug!: string
  articleComments$!: Observable<IComment[] | null>
  selectedArticle!: IArticle

  isUserAuthenticated!: boolean
  selectedArticleSubscription!: Subscription
  isUserAuthenticatedSubscription!: Subscription
  public get ButtonType() {
    return ButtonType
  }
  constructor(private store: Store, private route: ActivatedRoute, private router: Router) {}
  ngOnDestroy(): void {
    this.isUserAuthenticatedSubscription.unsubscribe()
    this.selectedArticleSubscription.unsubscribe()
  }

  ngOnInit(): void {
    this.selectedArticle$ = this.store.select(state =>
      selectorSelectedArticle(state as { [articleFeatureKey]: IArticleState })
    )
    this.isLoadingArticle$ = this.store.select(state =>
      selectorIsAnythingSubmittingInArticleModule(state as { [articleFeatureKey]: IArticleState })
    )
    this.isCurrUserArticleAuthor$ = this.store.select(state =>
      selectorIsCurrentUserAuthorOfSelectedArticle(
        state as { [articleFeatureKey]: IArticleState; [authFeatureKey]: IAuthState }
      )
    )

    this.selectedArticleSlug = this.route.snapshot.paramMap.get('slug') as string
    this.store.dispatch(
      articleActions.selectArticle({
        selectedArticleSlug: this.selectedArticleSlug as string,
        displayIsLoading: true,
      })
    )

    this.isUserAuthenticated$ = this.store.select(state =>
      selectorIsLoggedIn(state as { [authFeatureKey]: IAuthState })
    )
    this.isUserAuthenticatedSubscription = this.store
      .select(state => selectorIsLoggedIn(state as { [authFeatureKey]: IAuthState }))
      .subscribe(isAuthenticated => {
        if (isAuthenticated == null || isAuthenticated == undefined) {
          this.isUserAuthenticated = false
        }
        this.isUserAuthenticated = isAuthenticated as boolean
      })

    this.selectedArticleSubscription = this.selectedArticle$.subscribe(article => {
      this.selectedArticle = article as IArticle
    })

    this.articleComments$ = this.store.select(state =>
      selectorArticleComments(state as { [articleFeatureKey]: IArticleState })
    )
    this.store.dispatch(articleActions.reloadArticleComments({ articleSlug: this.selectedArticleSlug}))
  }

  //these are all duplicated from article-preview.component.ts, profile.component.ts => if we use component inheritance, we can avoid this
  onLikeButtonClicked(articleFavorited: boolean | null | undefined) {
    //if we do not pass in article.favorited to shared button at isArticleFavorited, it will be undefined
    if (articleFavorited === null || articleFavorited === undefined) {
      console.log('invalid state')
    }

    if (articleFavorited === true) {
      console.log('change to unfavorite')
      this.store.dispatch(
        homeActions.unfavoriteArticle({ articleSlug: this.selectedArticle.slug as string })
      )
    }

    if (articleFavorited === false) {
      console.log('change to favorite')
      this.store.dispatch(
        homeActions.favoriteArticle({ articleSlug: this.selectedArticle.slug as string })
      )
    }
  }

  unfollowProfile() {
    if (!this.isUserAuthenticated) {
      this.router.navigate(['/login'])
      return
    }
    this.store.dispatch(
      profileActions.unfollowProfile({ username: this.selectedArticle.author?.username as string })
    )
  }

  followProfile() {
    if (!this.isUserAuthenticated) {
      this.router.navigate(['/login'])
      return
    }
    this.store.dispatch(
      profileActions.followProfile({ username: this.selectedArticle.author?.username as string })
    )
  }

  deleteArticle() {
    this.store.dispatch(
      articleActions.deleteArticle({ articleSlug: this.selectedArticle.slug as string })
    )
  }
}
