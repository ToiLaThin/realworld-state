import { NgModule } from "@angular/core";
import { SharedModule } from "../shared/shared.module";
import { RouterModule } from "@angular/router";
import { articleRoutes } from "./article.routes";
import { ArticleComponent } from "./components/article.component";
import { MarkdownPipe } from "./markdown.pipe";

@NgModule({
    declarations: [ArticleComponent, MarkdownPipe],
    imports: [SharedModule, RouterModule.forChild(articleRoutes)],
    exports: []
})
export class ArticleModule {

}