export interface IFilter {
    limit: number;
    offset: number;
    tag: string | null;
    author: string | null;
    favorited: string | null;
}