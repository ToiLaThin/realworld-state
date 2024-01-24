import { NgModule, isDevMode } from "@angular/core";
import { AppComponent } from "./app.component";
import { BrowserModule } from "@angular/platform-browser";
import { CommonModule } from "@angular/common";
import { RouterModule } from "@angular/router";
import { appRoutes } from "./app.routes";
import { SharedModule } from "./shared/shared.module";
import { CoreModule } from "./core/core.module";
import { StoreModule } from "@ngrx/store";
import { StoreDevtoolsModule } from "@ngrx/store-devtools";
import { EffectsModule } from "@ngrx/effects";
import { AuthModule } from "./auth/auth.module";
import { homeFeatureKey, homeReducer } from "./state/home/home.reducers";
import { authFeatureKey, authReducer } from "./state/auth/auth.reducers";
import { HomeEffects } from "./state/home/home.effects";
import { AuthEffect } from "./state/auth/auth.effects";

@NgModule({
    declarations: [
        AppComponent
    ],
    imports: [
        BrowserModule,
        CommonModule,
        RouterModule.forRoot(appRoutes),
        SharedModule,
        CoreModule,
        AuthModule,
        StoreModule.forRoot({
            //each feature state is registered here
            //each feature state only have one reducer
            //mutiple module can perform actions modify one reducer
            //we can use action group to know of all actions in a feature state
            //which action belong to which module
            [homeFeatureKey]: homeReducer,
            [authFeatureKey]: authReducer
        }),
        StoreDevtoolsModule.instrument({
            maxAge: 25,
            logOnly: !isDevMode(),
            autoPause: true,
            traceLimit: 25
        }),
        EffectsModule.forRoot([HomeEffects, AuthEffect])
    ],
    providers: [],
    bootstrap: [AppComponent]
})
export class AppModule {}