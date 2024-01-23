import { Component } from '@angular/core'
import { RouterOutlet } from '@angular/router'
import { ButtonType } from './core/ui-models/button-types.enum'
import { IArticle } from './core/models/article.interface'

@Component({
  selector: 'rw-root',
  templateUrl: './app.component.html',
})
export class AppComponent {
  title = 'realworld-state-test'
}
