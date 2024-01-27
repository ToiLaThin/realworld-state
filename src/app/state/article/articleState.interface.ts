import { IArticle } from "../../core/models/article.interface";

export interface IArticleState {
    selectedArticle: IArticle | null;
    isAnythingSubmitting: boolean;    
}