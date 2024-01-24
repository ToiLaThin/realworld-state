import { NgModule } from "@angular/core";
import { SharedModule } from "../shared/shared.module";
import { RouterModule } from "@angular/router";
import { settingRoutes } from "./settings.routes";
import { SettingsComponent } from "./components/settings.component";
import { SettingsService } from "../core/services/settings.service";


@NgModule({
  declarations: [SettingsComponent],
  imports: [
    RouterModule.forChild(settingRoutes),
    SharedModule,
  ],
  providers: [SettingsService],
  exports: [],
})
export class SettingModule {}
