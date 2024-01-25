import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { IArticle } from "../models/article.interface";
import { environment as env } from "../../../environments/environment.development";
import { map } from "rxjs";
@Injectable({
    providedIn: 'root'
})
export class ArticleService {
    constructor(private http: HttpClient) {}

    createArticle(article: IArticle) {
        //an object with { key: value } pair
        return this.http.post<{article: IArticle}>(`${env.API_BASE_URL}/articles`, {article: article}).pipe(
            map((response: {article: IArticle}) => response.article)
        ) 
    }

    updateArticle(article: IArticle) {
        return this.http.put<{article: IArticle}>(`${env.API_BASE_URL}/articles/${article.slug}`, {article: article}).pipe(
            map((response: {article: IArticle}) => response.article)
        )
    }
}