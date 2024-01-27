import { NgModule } from "@angular/core";
import { SharedModule } from "../shared/shared.module";
import { RouterModule } from "@angular/router";
import { articleRoutes } from "./article.routes";
import { ArticleComponent } from "./components/article.component";
import { MarkdownPipe } from "./markdown.pipe";
import { CommentComponent } from "./components/comment.component";
import { CommentFormComponent } from "./components/comment-form.component";

@NgModule({
    declarations: [ArticleComponent, MarkdownPipe, CommentComponent, CommentFormComponent],
    imports: [SharedModule, RouterModule.forChild(articleRoutes)],
    exports: []
})
export class ArticleModule {

}