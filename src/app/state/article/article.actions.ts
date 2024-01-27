import { createActionGroup, props } from "@ngrx/store";
import { IArticle } from "../../core/models/article.interface";


export const articleActions = createActionGroup({
    source: 'Article Feature Module',
    events: {
        'Select Article': props<{ selectedArticleSlug: string }>(),
        'Load Article': props<{ selectedArticleSlug: string }>(),
        'Load Article Success': props<{ returnedArticle: IArticle }>(),
        'Load Article Failure': props<{ errors: any }>(),
    }
})