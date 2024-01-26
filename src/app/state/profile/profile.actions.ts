import { createActionGroup, props } from "@ngrx/store";
import { IProfile } from "../../core/models/profile.interface";
import { IErrors } from "../../core/ui-models/errors.interface";

export const profileActions = createActionGroup({
    source: 'Profile Feature Module',
    events: {
        'Profile Of User Selected': props<{ username: string }>(),
        'Get Profile Success': props<{ returnedProfile: IProfile }>(),
        'Get Profile Failure': props<{ errors: IErrors }>(),
        'Follow Profile': props<{ username: string }>(),
        'Unfollow Profile': props<{ username: string }>(),

        'Follow Or Unfollow Profile Success': props<{ returnedProfile: IProfile }>(),
        'Follow Or Unfollow Profile Failure': props<{ errors: IErrors }>(),
    }
})