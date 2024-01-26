import { IProfile } from "../../core/models/profile.interface";

export interface IProfileState {
    isLoadingProfile: boolean,
    viewingProfile: IProfile | null,
}