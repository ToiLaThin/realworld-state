import { createActionGroup, emptyProps, props } from "@ngrx/store";
import { IErrors } from "../../core/ui-models/errors.interface";


export const tagActions = createActionGroup({
    source: 'Home Feature',
    events: {
        'Get Tags': emptyProps(),
        'Get Tags Success': props<{ loadedTags: string[] }>(),
        'Get Tags Failure': props<{ errors: IErrors }>(),
    },
})