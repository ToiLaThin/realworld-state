import { createActionGroup, emptyProps, props } from "@ngrx/store";
import { IArticle } from "../../core/models/article.interface";
import { IErrors } from "../../core/ui-models/errors.interface";
import { IComment } from "../../core/models/comment.interface";


export const articleActions = createActionGroup({
    source: 'Article Feature Module',
    events: {
        'Select Article': props<{ selectedArticleSlug: string, displayIsLoading: boolean  }>(),
        'Load Article': props<{ selectedArticleSlug: string, displayIsLoading: boolean }>(),
        'Load Article Success': props<{ returnedArticle: IArticle }>(),
        'Load Article Failure': props<{ errors: IErrors }>(),

        'Delete Article': props<{ articleSlug: string }>(),
        'Delete Article Success': emptyProps(),
        'Delete Article Failure': props<{ errors: IErrors }>(),
        'Update Article': props<{ articleSlug: string }>(),

        'Reload Article Comments': props<{ articleSlug: string }>(),
        'Reload Article Comments Success': props<{ returnedComments: IComment[] }>(),
        'Reload Article Comments Failure': props<{ errors: IErrors }>(),
        'Add Comment To Article': props<{ articleSlug: string, commentBody: string }>(),
        'Delete Comment From Article': props<{ articleSlug: string, commentId: number }>(),
    }
})