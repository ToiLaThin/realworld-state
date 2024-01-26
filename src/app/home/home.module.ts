import { NgModule } from "@angular/core";
import { SharedModule } from "../shared/shared.module";
import { HomeComponent } from "./components/home.component";
import { RouterModule } from "@angular/router";
import { homeRoutes } from "./home.routes";
import { TagListComponent } from "./components/tag-list.component";
import { TagService } from "../core/services/tag.service";


@NgModule({
  declarations: [HomeComponent, TagListComponent],
  imports: [
    RouterModule.forChild(homeRoutes),
    SharedModule,
  ],
  providers: [TagService],
  exports: [],
})
export class HomeModule {}
