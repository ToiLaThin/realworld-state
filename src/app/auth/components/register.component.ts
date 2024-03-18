import { Component, OnInit } from "@angular/core";
import { FormBuilder, Validators } from "@angular/forms";
import { Store } from "@ngrx/store";
import { Observable } from "rxjs";
import { selectorHaveValidationErrors, selectorLoginValidationErrors } from "../../state/auth/auth.selectors";
import { IAuthState } from "../../state/auth/authState.interface";
import { authFeatureKey } from "../../state/auth/auth.reducers";
import { IRegisterRequest } from "../types/registerRequest.interface";
import { loginActions } from "../../state/auth/auth.actions";

@Component({
    selector: 'rw-register',
    templateUrl: './register.component.html'
})
export class RegisterComponent implements OnInit {
    haveRegisterErrors$!: Observable<boolean>
    registerErrors$!: Observable<object>
    isFormSubmitting$!: Observable<boolean>
    registerForm = this.fb.group({
        email: ['', Validators.required],
        username: ['', Validators.required],
        password: ['', Validators.required],
    })

    constructor(private store: Store, private fb: FormBuilder) {}

    ngOnInit(): void {
        this.haveRegisterErrors$ = this.store.select(state =>
          selectorHaveValidationErrors(state as { [authFeatureKey]: IAuthState })
        )
        this.registerErrors$ = this.store.select(
          state => selectorLoginValidationErrors(state as { [authFeatureKey]: IAuthState })
        )
        this.isFormSubmitting$ = this.store.select(
          state => (state as { [authFeatureKey]: IAuthState })[authFeatureKey].isSubmittingLoginRequest
        )
    }

    register() {
        const registerReq: IRegisterRequest = {
            email: this.registerForm.value.email!,
            username: this.registerForm.value.username!,
            password: this.registerForm.value.password!
        }
        this.store.dispatch(loginActions.register({ registerRequest: registerReq}))
    }
}