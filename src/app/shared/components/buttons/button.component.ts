import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core'
import { ButtonType } from '../../../core/ui-models/button-types.enum'
import { Store } from '@ngrx/store'
import { Observable } from 'rxjs'
import { homeFeatureKey } from '../../../state/home/home.reducers'
import { IHomeState } from '../../../state/home/homeState.interface'
import { selectorIsSubmittingFavoriteRequest } from '../../../state/home/home.selectors'

@Component({
  selector: 'rw-shared-button',
  templateUrl: './button.component.html',
})
export class SharedButtonComponent implements OnInit {
  @Input() displayText!: string
  @Input() buttonType!: ButtonType
  @Input() counter!: number
  @Input() isPullRight!: boolean
  @Input() isArticleFavorited!: boolean | null | undefined
  @Output() buttonClicked = new EventEmitter<boolean | null | undefined>()
  style() {
    return
  }

  public get ButtonType() {
    return ButtonType
  }

  public get likeButtonClass(): string {
    if (
      this.isArticleFavorited === null ||
      this.isArticleFavorited === undefined ||
      this.isArticleFavorited === false
    ) {
      return 'btn-outline-primary'
    }
    return 'btn-primary'
  }

  isAnyButtonSubmittingFavoriteReq$!: Observable<boolean>
  constructor(private store: Store) {}

  ngOnInit(): void {
    this.isAnyButtonSubmittingFavoriteReq$ = this.store.select(state =>
      selectorIsSubmittingFavoriteRequest(state as { [homeFeatureKey]: IHomeState })
    )
  }
}
