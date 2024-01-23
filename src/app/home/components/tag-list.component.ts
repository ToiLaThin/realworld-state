import { Component, OnInit } from "@angular/core";
import { Store } from "@ngrx/store";
import { Observable } from "rxjs";
import { selectorIsLoadingTags, selectorTags } from "../state/home.selectos";
import { homeFeatureKey } from "../state/home.reducers";
import { IHomeState } from "../types/homeState.interface";
import { tagActions } from "../state/home.actions";

@Component({
    selector: 'rw-tag-list',
    templateUrl: './tag-list.component.html'
})
export class TagListComponent implements OnInit{
    tags$!: Observable<string[]>;
    isLoadingTags$!: Observable<boolean>;
    
    constructor(private store: Store) {}

    ngOnInit(): void {
        this.tags$ = this.store.select((state) => selectorTags(state as { [homeFeatureKey]: IHomeState}))
        this.isLoadingTags$ = this.store.select((state) => selectorIsLoadingTags(state as { [homeFeatureKey]: IHomeState}))        
        this.store.dispatch(tagActions.getTags())
    }

    trackByFn(index: number, tag: string): string {
        return tag;
    }


}