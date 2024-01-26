import { Component, Input, OnInit } from "@angular/core";
import { Observable } from "rxjs";
import { IArticle } from "../../../core/models/article.interface";
import { Store } from "@ngrx/store";
import { selectorDisplayingArticles, selectorIsLoadingArticle } from "../../../state/home/home.selectors";
import { homeFeatureKey } from "../../../state/home/home.reducers";
import { IHomeState } from "../../../state/home/homeState.interface";

@Component({
    selector: 'rw-article-list',
    templateUrl: './article-list.component.html',
})
export class ArticleListComponent implements OnInit {
    //this is a component because it is shared between home and profile modules and many more
    displayingArticles$!: Observable<IArticle[] | null>
    isLoadingArticles$!: Observable<boolean>
    constructor(private store: Store) {}

    ngOnInit(): void {
        this.displayingArticles$ = this.store.select(state => selectorDisplayingArticles(state as { [homeFeatureKey]: IHomeState }))
        this.isLoadingArticles$ = this.store.select(state => selectorIsLoadingArticle(state as { [homeFeatureKey]: IHomeState }))        
    }

    //identify each article by its slug, trackBy is a performance optimization
    //it will only re-render the article that has changed
    trackByFn(index: number, article: IArticle) {
        return article.slug
    }
}