import { Component, OnInit } from '@angular/core'
import { Store } from '@ngrx/store'
import { selectorCurrentPage, selectorTotalArticlesCount } from '../../../state/home/home.selectors'
import { homeFeatureKey } from '../../../state/home/home.reducers'
import { IHomeState } from '../../../state/home/homeState.interface'
import { Observable, map } from 'rxjs'
import { homeActions } from '../../../state/home/home.actions'

@Component({
  selector: 'rw-paginator',
  templateUrl: './paginator.component.html',
})
export class PaginatorComponent implements OnInit {

  currentPage$!: Observable<number>
  pageNumArr$!: Observable<number[]> 

  constructor(private store: Store) {}
    ngOnInit(): void {
        this.currentPage$ = this.store.select(state => selectorCurrentPage(state as { [homeFeatureKey]: IHomeState }))
        //TODO make a selector for this in home.selectors.ts , do not do this in component
        this.pageNumArr$ = this.store  
        .select(state => selectorTotalArticlesCount(state as { [homeFeatureKey]: IHomeState }))
        .pipe(
          map(totalArticlesCount =>
            //Array.from freeCodeCamp for more info
            Array.from({ length: Math.ceil(totalArticlesCount / 10) }, (_, index) => index + 1)
          )
        )
    }

    selectPage(pageNum: number) {
        this.store.dispatch(homeActions.choosePage({ page: pageNum }))
    }
}
