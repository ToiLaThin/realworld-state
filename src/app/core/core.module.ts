import { NgModule } from "@angular/core";
import { HeaderComponent } from "./layout/header.component";
import { FooterComponent } from "./layout/footer.component";

@NgModule({
    declarations: [HeaderComponent, FooterComponent],
    imports: [],
    providers: [],
    exports: [HeaderComponent, FooterComponent]
})
export class CoreModule {

}