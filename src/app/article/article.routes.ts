import { Routes } from "@angular/router";
import { ArticleComponent } from "./components/article.component";

export const articleRoutes: Routes = [
    {
        path: ':slug',
        component: ArticleComponent
    }
]