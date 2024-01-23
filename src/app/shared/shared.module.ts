import { NgModule } from '@angular/core'
import { HeaderComponent } from '../core/layout/header.component'
import { FooterComponent } from '../core/layout/footer.component'
import { SharedButtonComponent } from './components/buttons/button.component'
import { CommonModule } from '@angular/common'
import { ArticleMetaComponent } from './components/article-shared-components/article-meta.component'
import { ArticlePreviewComponent } from './components/article-shared-components/article-preview.component'
import { HttpClientModule } from '@angular/common/http'
import { RouterModule } from '@angular/router'
import { FormsModule, ReactiveFormsModule } from '@angular/forms'
@NgModule({
  declarations: [SharedButtonComponent, ArticleMetaComponent, ArticlePreviewComponent],
  imports: [CommonModule, HttpClientModule, RouterModule],
  providers: [],
  exports: [
    RouterModule,
    CommonModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    ArticleMetaComponent,
    ArticlePreviewComponent,
    SharedButtonComponent,
  ],
})
export class SharedModule {}
