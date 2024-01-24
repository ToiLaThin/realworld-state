import { createReducer, on } from "@ngrx/store"
import { IHomeState } from "./homeState.interface"
import { tagActions } from "./home.actions"

export const initialHomeState: IHomeState = {
    tags: [],
    isLoadingTags: false,
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
    on(
        tagActions.getTagsSuccess,
        (state, action) => ({
            ...state,
            tags: action.loadedTags,
            isLoadingTags: false,
        })
    ),
    on(
        tagActions.getTagsFailure,
        (state): IHomeState => ({
            ...state,
            isLoadingTags: false,
        })
    )
)