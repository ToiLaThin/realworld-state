import { createReducer, on } from "@ngrx/store";
import { IArticleState } from "./articleState.interface";
import { articleActions } from "./article.actions";


export const initialArticleState : IArticleState = {
    selectedArticle: null,
    isAnythingSubmitting: false,

}
export const articleFeatureKey = 'article';
export const articleReducer = createReducer(
    initialArticleState,
    //this mean that the return type of the reducer is IArticleState
    on(articleActions.selectArticle, (state, action): IArticleState => ({
        ...state,
        selectedArticle: null,
    })),
    on(articleActions.loadArticle, (state, action): IArticleState => ({
        ...state,
        isAnythingSubmitting: true,
    })),
    on(articleActions.loadArticleSuccess, (state, action): IArticleState => ({
        ...state,
        isAnythingSubmitting: false,
        selectedArticle: action.returnedArticle,
    })),
    on(articleActions.loadArticleFailure, (state, action): IArticleState => ({
        ...state,
        isAnythingSubmitting: false,
    })),
)