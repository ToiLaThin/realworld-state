import { HttpClient, HttpParams } from '@angular/common/http'
import { Injectable } from '@angular/core'
import { IArticle } from '../models/article.interface'
import { environment as env } from '../../../environments/environment.development'
import { Observable, map } from 'rxjs'
import { IFilter } from '../models/filter.interface'
import { FeedType } from '../ui-models/feed-types.enum'
@Injectable({
  providedIn: 'root',
})
export class ArticleService {
  constructor(private http: HttpClient) {}

  createArticle(article: IArticle) {
    //an object with { key: value } pair
    return this.http
      .post<{ article: IArticle }>(`${env.API_BASE_URL}/articles`, { article: article })
      .pipe(map((response: { article: IArticle }) => response.article))
  }

  updateArticle(article: IArticle) {
    return this.http
      .put<{ article: IArticle }>(`${env.API_BASE_URL}/articles/${article.slug}`, {
        article: article,
      })
      .pipe(map((response: { article: IArticle }) => response.article))
  }

  private convertFilterToHttpParams(filter: IFilter): HttpParams {
    if (filter === null || filter === undefined) {
      alert('filter is null or undefined')
    }
    let httpParams = new HttpParams()
    let filterObject = Object(filter) //convert to object to use Object.keys
    console.log('Filter as object at article service convertFilterToHttpParams:', filterObject)
    Object.keys(filter).forEach((key: string) => {
      if (filterObject[key] || filterObject[key] === 0) {
        //do not discard offset 0
        httpParams = httpParams.append(key, filterObject[key])
      }
    })
    console.log(httpParams.keys())
    return httpParams
  }

  //divide to 2 function, one for global feed, one for user feed or one function with 2 parameter
  // if filter is of type object we can use new HttpParams({fromObject: filter})
  listArticles(
    filter: IFilter,
    feedType: FeedType
  ): Observable<{ articles: IArticle[]; articlesCount: number }> {
    let endpoint = 'articles'
    if (feedType === FeedType.USER) {
      endpoint = 'articles/feed'
    }

    let httpParams = this.convertFilterToHttpParams(filter)
    return this.http
      .get<{ articles: IArticle[]; articlesCount: number }>(`${env.API_BASE_URL}/${endpoint}`, {
        params: httpParams,
      })
      .pipe(
        map((response: { articles: IArticle[]; articlesCount: number }) => ({
          articles: response.articles,
          articlesCount: response.articlesCount,
        }))
      )
  }

  favoriteArticle(slug: string): Observable<IArticle> {
    return this.http
      .post<{ article: IArticle }>(`${env.API_BASE_URL}/articles/${slug}/favorite`, {})
      .pipe(map((response: { article: IArticle }) => response.article))
  }

  unfavoriteArticle(slug: string): Observable<IArticle> {
    return this.http
      .delete<{ article: IArticle }>(`${env.API_BASE_URL}/articles/${slug}/favorite`)
      .pipe(map((response: { article: IArticle }) => response.article))
  }

  getArticle(slug: string): Observable<IArticle> {
    return this.http
      .get<{ article: IArticle }>(`${env.API_BASE_URL}/articles/${slug}`)
      .pipe(map((response: { article: IArticle }) => response.article))
  }
}
