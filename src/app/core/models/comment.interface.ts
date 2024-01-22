import { IProfile } from "./profile.interface";

export interface IComment {
    id: number;
    createdAt: Date;
    updatedAt: Date;
    body: string;
    author: IProfile;
}