import { createFeatureSelector, createSelector } from '@ngrx/store'
import { articleFeatureKey } from './article.reducers'
import { IArticleState } from './articleState.interface'
import { IAuthState } from '../auth/authState.interface'
import { authFeatureKey } from '../auth/auth.reducers'

export const selectArticleFeature = (state: { [articleFeatureKey]: IArticleState }) =>
  state[articleFeatureKey]

export const selectorSelectedArticle = createSelector(
    selectArticleFeature,
    articleState => articleState.selectedArticle
)

export const selectorIsAnythingSubmittingInArticleModule = createSelector(
    selectArticleFeature,
    articleState => articleState.isAnythingSubmitting
)

const selectAuthFeatureLocal = createFeatureSelector<IAuthState>(authFeatureKey)
const selectArticleFeatureLocal = createFeatureSelector<IArticleState>(articleFeatureKey)
export const selectorIsCurrentUserAuthorOfSelectedArticle = createSelector(
  selectAuthFeatureLocal,
  selectArticleFeatureLocal,
  (authState, articleState) => {
    return authState.currentUser?.username === articleState.selectedArticle?.author
  }
)