import { Component, OnInit } from '@angular/core'
import { Observable } from 'rxjs'
import { IArticle } from '../../core/models/article.interface'
import { Store } from '@ngrx/store'
import {
  selectorIsAnythingSubmittingInArticleModule,
  selectorIsCurrentUserAuthorOfSelectedArticle,
  selectorSelectedArticle,
} from '../../state/article/article.selectors'
import { articleFeatureKey } from '../../state/article/article.reducers'
import { IArticleState } from '../../state/article/articleState.interface'
import { authFeatureKey } from '../../state/auth/auth.reducers'
import { IAuthState } from '../../state/auth/authState.interface'
import { articleActions } from '../../state/article/article.actions'
import { ActivatedRoute } from '@angular/router'

@Component({
  selector: 'rw-article',
  templateUrl: './article.component.html',
})
export class ArticleComponent implements OnInit {
  selectedArticle$!: Observable<IArticle | null>
  isLoadingArticle$!: Observable<boolean>
  isCurrUserArticleAuthor$!: Observable<boolean>
  constructor(private store: Store, private route: ActivatedRoute) {}

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

    let selectedArticleSlug = this.route.snapshot.paramMap.get('slug')
    this.store.dispatch(
      articleActions.selectArticle({ selectedArticleSlug: selectedArticleSlug as string })
    )
  }
}
