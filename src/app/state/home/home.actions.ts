import { createActionGroup, emptyProps, props } from "@ngrx/store";
import { IErrors } from "../../core/ui-models/errors.interface";
import { IArticle } from "../../core/models/article.interface";
import { FeedType } from "../../core/ui-models/feed-types.enum";
import { IFilter } from "../../core/models/filter.interface";


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

export const homeActions = createActionGroup({
    source: 'Home Feature Module',
    events: {
        
        'Choose Page': props<{page: number}>(), //set page will change current page and filter offset, base on current page + limit
        'Set Feed Type': props<{feedType: FeedType}>(),

        //action should be named as event, because it is an event, not a command
        'Filter By Tag Changed': props<{tag: string}>(),
        'Filter By Author Changed': props<{author: string}>(),
        'Filter By Favorited Changed': props<{favorited: string}>(),
        
        //'Filter Changed': props<{newFilterBy: IFilter}>(), 
        //We can have many small event like FilterByTag, FilterByAuthor, FilterByFavorited 
        //And we should, because if we have one large event with many different case of filter, like filter by author, favorited, tag, 
        //Component which dispatch this event will have to select from store, and check which filter is changed, and dispatch the right event, which is more complex)

        //init will dispatch load articles with init home state
        'Reload Articles': emptyProps(),
        'Load Articles Success': props<{loadedArticles: IArticle[], totalArticlesCount: number}>(),
        'Load Articles Failure': props<{errors: IErrors}>(),


        'Favorite Article': props<{articleSlug: string}>(),
        'Unfavorite Article': props<{articleSlug: string}>(),
        'Favorite Or Unfavorite Article Success': props<{returnArticle: IArticle}>(),
        'Favorite Or Unfavorite Article Failure': props<{errors: IErrors}>(),
    }
})