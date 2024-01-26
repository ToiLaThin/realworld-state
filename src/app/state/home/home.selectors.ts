import { state } from '@angular/animations'
import { homeFeatureKey } from './home.reducers'
import { IHomeState } from './homeState.interface'
import { createSelector } from '@ngrx/store'

export const selectHomeFeature = (state: { [homeFeatureKey]: IHomeState }) => state[homeFeatureKey]
export const selectorTags = createSelector(selectHomeFeature, homeState => homeState.tags)
export const selectorIsLoadingTags = createSelector(
  selectHomeFeature,
  homeState => homeState.isLoadingTags
)
export const selectorIsEditorFormSubmitting = createSelector(
  selectHomeFeature,
  homeState => homeState.isEditorFormSubmitting
)

export const selectorDisplayingArticles = createSelector(
  selectHomeFeature,
  homeState => homeState.displayingArticles
)

export const selectorTotalArticlesCount = createSelector(
  selectHomeFeature,
  homeState => homeState.totalArticlesCount
)

export const selectorIsLoadingArticle = createSelector(
  selectHomeFeature,
  homeState => homeState.isLoadingArticle
)

export const selectorFeedType = createSelector(selectHomeFeature, homeState => homeState.feedType)

export const selectorCurrentPage = createSelector(
  selectHomeFeature,
  homeState => homeState.currentPage
)
export const selectorFilterBy = createSelector(selectHomeFeature, homeState => homeState.filterBy)

export const selectorIsOnProfileFavoritesTab = createSelector(
  selectHomeFeature,
  homeState =>
    homeState.filterBy.favorited !== undefined &&
    homeState.filterBy.favorited !== null &&
    homeState.filterBy.favorited !== ''
)

export const selectorIsSubmittingFavoriteRequest = createSelector(
  selectHomeFeature,
  homeState => homeState.isSubmittingFavorite
)
