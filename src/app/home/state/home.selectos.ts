import { state } from "@angular/animations";
import { homeFeatureKey } from "./home.reducers";
import { IHomeState } from './../types/homeState.interface';
import { createSelector } from "@ngrx/store";

export const selectHomeFeature = (state: { [homeFeatureKey]: IHomeState}) => state[homeFeatureKey]
export const selectorTags = createSelector(
    selectHomeFeature,
    homeState => homeState.tags
)
export const selectorIsLoadingTags = createSelector(
    selectHomeFeature,
    homeState => homeState.isLoadingTags
)