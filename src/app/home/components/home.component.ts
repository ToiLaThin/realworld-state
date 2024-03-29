import { Component, OnInit } from '@angular/core'
import { Store } from '@ngrx/store'
import { Observable, Subscription } from 'rxjs'
import { selectorIsLoggedIn } from '../../state/auth/auth.selectors'
import { IAuthState } from '../../state/auth/authState.interface'
import { authFeatureKey } from '../../state/auth/auth.reducers'
import { ButtonType } from '../../core/ui-models/button-types.enum'
import { IArticle } from '../../core/models/article.interface'
import { homeActions } from '../../state/home/home.actions'
import { FeedType } from '../../core/ui-models/feed-types.enum'
import { selectorFeedType, selectorSelectedTag, selectorTags } from '../../state/home/home.selectors'
import { IHomeState } from '../../state/home/homeState.interface'
import { homeFeatureKey } from '../../state/home/home.reducers'
import { Router } from '@angular/router'

@Component({
  selector: 'rw-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnInit {
  isLoggedIn$!: Observable<boolean | null>
  selectedFeedType$!: Observable<FeedType>


  selectedTag$!: Observable<string | null>
  isLoggedIn!: boolean
  isLoggedInSubscription!: Subscription

  public get ButtonType() {
    return ButtonType
  }
  public get FeedType() {
    return FeedType
  }

  constructor(private store: Store, private router: Router) {}

  ngOnInit(): void {
    this.isLoggedIn$ = this.store.select(state =>
      selectorIsLoggedIn(state as { [authFeatureKey]: IAuthState })
    )
    this.selectedFeedType$ = this.store.select(state => selectorFeedType(state as { [homeFeatureKey]: IHomeState }))
    //init load articles
    this.store.dispatch(homeActions.reloadArticles())
    this.isLoggedInSubscription = this.isLoggedIn$.subscribe(isLoggedIn => {
      this.isLoggedIn = isLoggedIn!
    })

    this.selectedTag$ = this.store.select(state => selectorSelectedTag(state as { [homeFeatureKey]: IHomeState }))
  }

  switchToPersonalFeed() {
    if (!this.isLoggedIn) {
      this.router.navigateByUrl('/login')
      return
    }
    this.store.dispatch(homeActions.setFeedType({ feedType: FeedType.USER }))
  }

  switchToGlobalFeed() {
    this.store.dispatch(homeActions.setFeedType({ feedType: FeedType.GLOBAL }))
  }
}
