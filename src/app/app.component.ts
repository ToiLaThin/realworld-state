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

  testArticle: IArticle = {
      slug: "how-to-train-your-dragon",
      title: "How to train your dragon",
      description: "Ever wonder how?",
      body: "It takes a Jacobian",
      tagList: ["dragons", "training"],
      createdAt: "2016-02-18T03:22:56.637Z",
      updatedAt: "2016-02-18T03:48:35.824Z",
      favorited: false,
      favoritesCount: 0,
      author: {
        username: "jake",
        bio: "I work at statefarm",
        image: "https://i.stack.imgur.com/xHWG8.jpg",
        following: false
      }
  }
  
  public get ButtonType() {
    return ButtonType
  }
}
