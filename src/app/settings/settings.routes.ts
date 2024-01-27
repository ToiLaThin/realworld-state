import { Routes } from "@angular/router";
import { SettingsComponent } from "./components/settings.component";
import { AuthGuard } from "../core/guards/auth.guard";

export const settingRoutes : Routes = [
    {
        path: '',
        component: SettingsComponent,
        canActivate: [AuthGuard]        
    }
]