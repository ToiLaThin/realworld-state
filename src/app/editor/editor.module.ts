import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";
import { editorRoutes } from "./editor.routes";
import { SharedModule } from "../shared/shared.module";
import { ReactiveFormsModule } from "@angular/forms";
import { EditorComponent } from "./components/editor.component";

@NgModule({
    declarations: [EditorComponent],
    imports: [RouterModule.forChild(editorRoutes), SharedModule],
    providers: [],
    bootstrap: []
})
export class EditorModule {

}