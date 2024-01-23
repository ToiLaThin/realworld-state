import { NgModule } from "@angular/core";
import { SharedModule } from "../shared/shared.module";
import { HomeComponent } from "./components/home.component";
import { RouterModule } from "@angular/router";
import { homeRoutes } from "./home.routes";
import { PaginatorComponent } from "./components/paginator.component";
import { TagListComponent } from "./components/tag-list.component";
import { homeFeatureKey, homeReducer } from "./state/home.reducers";
import { StoreModule } from "@ngrx/store";
import { HomeEffects } from "./state/home.effects";
import { EffectsModule } from "@ngrx/effects";
import { TagService } from "./services/tag.service";


@NgModule({
  declarations: [HomeComponent, PaginatorComponent, TagListComponent],
  imports: [
    StoreModule.forFeature(homeFeatureKey, homeReducer),
    EffectsModule.forFeature([HomeEffects]),
    RouterModule.forChild(homeRoutes),
    SharedModule,
  ],
  providers: [TagService],
  exports: [],
})
export class HomeModule {}
