import { Routes } from "@angular/router";
import { EditorComponent } from "./components/editor.component";
import { AuthGuard } from "../core/guards/auth.guard";

export const editorRoutes: Routes = [
    {
        path: '',
        component: EditorComponent,
        canActivate: [AuthGuard]
    }
]
