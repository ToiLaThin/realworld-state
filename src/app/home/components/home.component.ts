import { Component, OnInit } from '@angular/core'
import { Store } from '@ngrx/store'
import { Observable } from 'rxjs'
import { selectorIsLoggedIn } from '../../state/auth/auth.selectors'
import { IAuthState } from '../../state/auth/authState.interface'
import { authFeatureKey } from '../../state/auth/auth.reducers'
import { ButtonType } from '../../core/ui-models/button-types.enum'
import { IArticle } from '../../core/models/article.interface'
import { homeActions } from '../../state/home/home.actions'
import { FeedType } from '../../core/ui-models/feed-types.enum'

@Component({
  selector: 'rw-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnInit {
  isLoggedIn$!: Observable<boolean | null>

  public get ButtonType() {
    return ButtonType
  }

  constructor(private store: Store) {}

  ngOnInit(): void {
    this.isLoggedIn$ = this.store.select(state =>
      selectorIsLoggedIn(state as { [authFeatureKey]: IAuthState })
    )
    //init load articles
    this.store.dispatch(homeActions.reloadArticles())
  }

  switchToPersonalFeed() {
    this.store.dispatch(homeActions.setFeedType({ feedType: FeedType.USER }))
  }

  switchToGlobalFeed() {
    this.store.dispatch(homeActions.setFeedType({ feedType: FeedType.GLOBAL }))
  }
}
