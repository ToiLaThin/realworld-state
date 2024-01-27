import { IArticle } from "../../core/models/article.interface";
import { IComment } from "../../core/models/comment.interface";

export interface IArticleState {
    selectedArticle: IArticle | null;
    isAnythingSubmitting: boolean;    

    displayingComments: IComment[] | [];
}