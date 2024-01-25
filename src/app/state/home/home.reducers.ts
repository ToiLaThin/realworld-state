import { createReducer, on } from "@ngrx/store"
import { IHomeState } from "./homeState.interface"
import { editorActions, tagActions } from "./home.actions"

export const initialHomeState: IHomeState = {
    tags: [],
    isLoadingTags: false,
    isEditorFormSubmitting: false,
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

)