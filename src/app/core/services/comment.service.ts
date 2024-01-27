import { HttpClient } from '@angular/common/http'
import { Injectable } from '@angular/core'
import { Observable, map } from 'rxjs'
import { IComment } from '../models/comment.interface'
import { environment as env } from '../../../environments/environment.development'
@Injectable({
  providedIn: 'root',
})
export class CommentService {
  constructor(private http: HttpClient) {}

  getArticleComments(slug: string): Observable<IComment[]> {
    return this.http
      .get<{ comments: IComment[] }>(`${env.API_BASE_URL}/articles/${slug}/comments`)
      .pipe(
        map(response => {
          return response.comments
        })
      )
  }

  addCommentToArticle(articleSlug: string, comment: string): Observable<IComment> {
    return this.http
      .post<{ comment: IComment }>(`${env.API_BASE_URL}/articles/${articleSlug}/comments`, {
        comment: { body: comment },
      })
      .pipe(
        map(response => {
          return response.comment
        })
      )
  }

  deleteCommentFromArticle(articleSlug: string, commentId: number): Observable<void> {
    return this.http.delete<void>(
      `${env.API_BASE_URL}/articles/${articleSlug}/comments/${commentId}`
    )
  }
}
