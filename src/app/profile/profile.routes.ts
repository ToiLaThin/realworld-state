import { Routes } from "@angular/router";
import { ProfileComponent } from "./components/profile.component";
import { UserNameProfileResolver } from "./username-profile.resolver";

export const profileRoutes: Routes = [
    {
        path: ':username',
        component: ProfileComponent,
        // path: 'profile/:username', before render component
        // use this resolver to get profile data of that username param
        // resolve: {
        //     usernameProfile: UserNameProfileResolver
        // }
    },
]