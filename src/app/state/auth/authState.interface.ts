import { IUser } from "../../core/models/user.interface";
import { IErrors } from "../../core/ui-models/errors.interface";

export interface IAuthState {
    isSubmittingLoginRequest: boolean;
    currentUser: IUser | null;
    isLoggedIn: boolean | null;
    validationErrors: IErrors | null;
}