import { Component, Input, OnInit } from '@angular/core'
import { IArticle } from '../../../core/models/article.interface'
import { ButtonType } from '../../../core/ui-models/button-types.enum'
import { Store } from '@ngrx/store'
import { homeActions } from '../../../state/home/home.actions'

@Component({
  selector: 'rw-article-preview',
  templateUrl: './article-preview.component.html',
})
//these component in shared because they are used in multiple places (from UI)
export class ArticlePreviewComponent {
  @Input() article!: IArticle
  public ButtonType = ButtonType

  constructor(private store: Store) {}

  onLikeButtonClicked(articleFavorited: boolean | null | undefined) {
    //if we do not pass in article.favorited to shared button at isArticleFavorited, it will be undefined
    if (articleFavorited === null || articleFavorited === undefined) {
      console.log('invalid state')
    }

    if (articleFavorited === true) {
      console.log('change to unfavorite')
      this.store.dispatch(
        homeActions.unfavoriteArticle({ articleSlug: this.article.slug as string })
      )
    }

    if (articleFavorited === false) {
      console.log('change to favorite')
      this.store.dispatch(homeActions.favoriteArticle({ articleSlug: this.article.slug as string }))
    }
  }
}
