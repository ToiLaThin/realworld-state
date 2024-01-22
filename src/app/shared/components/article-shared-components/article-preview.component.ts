import { Component, Input } from '@angular/core';
import { IArticle } from '../../../core/models/article.interface';
import { ButtonType } from '../../../core/ui-models/button-types.enum';
@Component({
    selector: 'rw-article-preview',
    templateUrl: './article-preview.component.html'
})
//these component in shared because they are used in multiple places (from UI)
export class ArticlePreviewComponent {
    @Input() article!: IArticle;
    public ButtonType = ButtonType;
}