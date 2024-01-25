import { IProfile } from './profile.interface'
//if other props is null, this is used as a request to create article, otherwise it is a request to update article or an article getted
export interface IArticle {
    slug: string | null;
    title: string;
    description: string;
    body: string;
    tagList: string[];
    createdAt: string | null;
    updatedAt: string | null;
    favorited: boolean | null;
    favoritesCount: number | null;
    author: IProfile | null;
}