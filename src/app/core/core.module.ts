import { NgModule } from "@angular/core";
import { HeaderComponent } from "./layout/header.component";
import { FooterComponent } from "./layout/footer.component";
import { RouterModule } from "@angular/router";
import { SharedModule } from "../shared/shared.module";

@NgModule({
    declarations: [HeaderComponent, FooterComponent],
    imports: [SharedModule],
    providers: [],
    exports: [HeaderComponent, FooterComponent]
})
export class CoreModule {

}