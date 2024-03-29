import { NgModule } from '@angular/core'
import { SharedButtonComponent } from './components/buttons/button.component'
import { CommonModule } from '@angular/common'
import { ArticleMetaComponent } from './components/article-shared-components/article-meta.component'
import { ArticlePreviewComponent } from './components/article-shared-components/article-preview.component'
import { HttpClientModule } from '@angular/common/http'
import { RouterModule } from '@angular/router'
import { FormsModule, ReactiveFormsModule } from '@angular/forms'
import { ArticleListComponent } from './components/article-shared-components/article-list.component'
import { PaginatorComponent } from './components/article-shared-components/paginator.component'
@NgModule({
  declarations: [
    SharedButtonComponent,
    ArticleMetaComponent,
    ArticlePreviewComponent,
    ArticleListComponent,
    PaginatorComponent,
  ],
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
    ArticleListComponent,
    SharedButtonComponent,
    PaginatorComponent,
  ],
})
export class SharedModule {}
