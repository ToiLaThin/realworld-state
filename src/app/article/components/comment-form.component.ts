import { ChangeDetectionStrategy, Component, Input, OnInit } from '@angular/core'
import { FormBuilder, FormGroup, Validators } from '@angular/forms'
import { Store } from '@ngrx/store'
import { articleActions } from '../../state/article/article.actions'

@Component({
  selector: 'rw-comment-form',
  templateUrl: './comment-form.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CommentFormComponent implements OnInit {
  @Input() articleSlug!: string
  commentForm = this.fb.group({
    commentBody: [''], //cannot use validators.required , will have errors
  })

  constructor(private fb: FormBuilder, private store: Store) {}

  ngOnInit(): void {}

  submitComment() {
    const commentBody = this.commentForm.value.commentBody
    this.store.dispatch(articleActions.addCommentToArticle({ articleSlug: this.articleSlug, commentBody: commentBody as string }))
    this.commentForm.reset()
  }
}
