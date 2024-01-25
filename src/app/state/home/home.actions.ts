import { createActionGroup, emptyProps, props } from "@ngrx/store";
import { IErrors } from "../../core/ui-models/errors.interface";
import { IArticle } from "../../core/models/article.interface";


export const tagActions = createActionGroup({
    source: 'Home Feature Module',
    events: {
        'Get Tags': emptyProps(),
        'Get Tags Success': props<{ loadedTags: string[] }>(),
        'Get Tags Failure': props<{ errors: IErrors }>(),
    },
})

//no state change, only side effect
export const editorActions = createActionGroup({
    source: 'Editor Feature Module',
    events: {
        'Submit Editor Form': props<{article: IArticle}>(),
        'Create New Article': props<{article: IArticle}>(),
        'Update Article': props<{article: IArticle}>(),
        'Submit Editor Form Success': props<{submittedArticle: IArticle}>(),
        'Submit Editor Form Failure': props<{errors: IErrors}>(),
    }
})