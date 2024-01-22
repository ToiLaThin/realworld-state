import { Component, Input } from "@angular/core";
import { IArticle } from "../../../core/models/article.interface";
@Component({
    selector: 'rw-article-meta',
    templateUrl: './article-meta.component.html'
})
export class ArticleMetaComponent {
    @Input() article!: IArticle;
}