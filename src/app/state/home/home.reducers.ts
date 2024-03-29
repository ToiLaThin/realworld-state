import { createReducer, on } from '@ngrx/store'
import { IHomeState } from './homeState.interface'
import { editorActions, homeActions, tagActions } from './home.actions'
import { FeedType } from '../../core/ui-models/feed-types.enum'

export const initialHomeState: IHomeState = {
  tags: [],
  isLoadingTags: false,
  isEditorFormSubmitting: false,

  totalArticlesCount: 0,
  displayingArticles: null,
  isLoadingArticle: false,
  feedType: FeedType.GLOBAL,
  currentPage: 1,
  filterBy: {
    limit: 10,
    offset: 0,
    tag: null,
    author: null,
    favorited: null,
  },

  isSubmittingFavorite: false,
}
export const homeFeatureKey = 'home'
export const homeReducer = createReducer(
  initialHomeState,
  on(
    tagActions.getTags,
    (state): IHomeState => ({
      ...state,
      isLoadingTags: true,
    })
  ),
  on(tagActions.getTagsSuccess, (state, action) => ({
    ...state,
    tags: action.loadedTags,
    isLoadingTags: false,
  })),
  on(
    tagActions.getTagsFailure,
    (state): IHomeState => ({
      ...state,
      isLoadingTags: false,
    })
  ),
  on(
    editorActions.submitEditorForm,
    (state): IHomeState => ({
      ...state,
      isEditorFormSubmitting: true,
    })
  ),
  on(
    editorActions.submitEditorFormSuccess,
    (state): IHomeState => ({
      ...state,
      isEditorFormSubmitting: false,
    })
  ),
  //on editorActions.submitEditorFormFailure we will modify errors state in another reducer or sub state
  //in this reducer, it just reset the isEditorFormSubmitting state to be false
  on(
    editorActions.submitEditorFormFailure,
    (state): IHomeState => ({
      ...state,
      isEditorFormSubmitting: false,
    })
  ),

  //reloadArticles will cause effect to call service to load articles
  //action won't have any props, because that will make component have logic to pass props to action
  //instead, we will get the filterBy, feedType from store in the effect
  on(homeActions.reloadArticles, (state): IHomeState => ({ ...state, isLoadingArticle: true })),
  on(
    homeActions.reloadArticlesWithoutDisplayIsLoading,
    (state): IHomeState => ({ ...state, isLoadingArticle: false })
  ),
  on(homeActions.loadArticlesSuccess, (state, action) => ({
    ...state,
    isLoadingArticle: false,
    totalArticlesCount: action.totalArticlesCount,
    displayingArticles: action.loadedArticles,
  })),
  on(
    homeActions.loadArticlesFailure,
    (state): IHomeState => ({ ...state, isLoadingArticle: false })
  ),

  //caused effect to reload page
  on(homeActions.choosePage, (state, action) => {
    //complex clone state
    const newOffset = (action.page - 1) * state.filterBy.limit
    return {
      ...state,
      currentPage: action.page,
      filterBy: { ...state.filterBy, offset: newOffset },
    }
  }),
  on(homeActions.setFeedType, (state, action) => ({
    ...state,
    //must also reset the page ortherwise, the query result might be blank
    //must also reset the offset ortherwise, the query result might be blank, because if only change the page num, the previous offset will still be the same => the wrong query
    currentPage: 1,
    feedType: action.feedType,
    filterBy: { ...state.filterBy, author: null, favorited: null, tag: null, offset: 0 },
  })),
  on(homeActions.filterByAuthorChanged, (state, action) => ({
    ...state,
    currentPage: 1,
    filterBy: { ...state.filterBy, author: action.author, favorited: null, tag: null, offset: 0 },
  })),
  on(homeActions.filterByFavoritedChanged, (state, action) => ({
    ...state,
    currentPage: 1,
    filterBy: {
      ...state.filterBy,
      favorited: action.favorited,
      author: null,
      tag: null,
      offset: 0,
    },
  })),
  on(homeActions.filterByTagChanged, (state, action) => ({
    ...state,
    currentPage: 1,
    filterBy: { ...state.filterBy, tag: action.tag, author: null, favorited: null, offset: 0 },
  })),

  on(homeActions.favoriteArticle, (state, action) => ({
    ...state,
    isSubmittingFavorite: true,
  })),
  on(homeActions.unfavoriteArticle, (state, action) => ({
    ...state,
    isSubmittingFavorite: true,
  })),
  on(homeActions.favoriteOrUnfavoriteArticleSuccess, (state, action) => ({
    ...state,
    isSubmittingFavorite: false,
  })),
  on(homeActions.favoriteOrUnfavoriteArticleFailure, (state, action) => ({
    ...state,
    isSubmittingFavorite: false,
  }))
)
