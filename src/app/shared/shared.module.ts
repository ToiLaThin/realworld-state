import { NgModule } from '@angular/core'
import { HeaderComponent } from '../core/layout/header.component'
import { FooterComponent } from '../core/layout/footer.component'
import { SharedButtonComponent } from './components/buttons/button.component'
import { CommonModule } from '@angular/common'
import { ArticleMetaComponent } from './components/article-shared-components/article-meta.component'
import { ArticlePreviewComponent } from './components/article-shared-components/article-preview.component'
@NgModule({
  declarations: [SharedButtonComponent, ArticleMetaComponent, ArticlePreviewComponent],
  imports: [CommonModule],
  providers: [],
  exports: [SharedButtonComponent, ArticleMetaComponent, ArticlePreviewComponent],
})
export class SharedModule {}
