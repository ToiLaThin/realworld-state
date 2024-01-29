import { ChangeDetectionStrategy, Component, Input, OnDestroy, OnInit } from '@angular/core'
import { IComment } from '../../core/models/comment.interface'
import { Store } from '@ngrx/store'
import { Observable, Subscription } from 'rxjs'
import { articleActions } from '../../state/article/article.actions'
import { IUser } from '../../core/models/user.interface'
import { selectorCurrentUser } from '../../state/auth/auth.selectors'
import { authFeatureKey } from '../../state/auth/auth.reducers'
import { IAuthState } from '../../state/auth/authState.interface'

@Component({
  selector: 'rw-comment',
  templateUrl: './comment.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CommentComponent implements OnInit, OnDestroy {
  @Input() comment!: IComment
  @Input() articleSlug!: string
  //   isCurrentUserCommentAuthor$!: Observable<boolean>

  currUser$!: Observable<IUser | null>
  isCurrentUserCommentAuthor!: boolean
  currUserSubscription!: Subscription
  constructor(private store: Store) {}

  ngOnDestroy(): void {
    this.currUserSubscription.unsubscribe()
  }

  ngOnInit(): void {
    this.currUser$ = this.store.select(state =>
      selectorCurrentUser(state as { [authFeatureKey]: IAuthState })
    )
    this.currUserSubscription = this.currUser$.subscribe(currUser => {
      if (currUser === null) {
        return
      }
      this.isCurrentUserCommentAuthor = currUser.username === this.comment.author.username
    })
  }

  deleteThisComment(commentId: number) {
    this.store.dispatch(
      articleActions.deleteCommentFromArticle({
        articleSlug: this.articleSlug,
        commentId: commentId,
      })
    )
  }
}
