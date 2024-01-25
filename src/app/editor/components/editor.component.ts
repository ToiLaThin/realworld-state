import { Component } from "@angular/core";
import { FormBuilder, Validators } from "@angular/forms";
import { Store } from "@ngrx/store";
import { IArticle } from "../../core/models/article.interface";
import { editorActions } from "../../state/home/home.actions";
import { selectorIsEditorFormSubmitting } from "../../state/home/home.selectos";
import { homeFeatureKey } from "../../state/home/home.reducers";
import { IHomeState } from "../../state/home/homeState.interface";
import { selectorHaveValidationErrors, selectorValidationErrors } from "../../state/auth/auth.selectors";
import { authFeatureKey } from "../../state/auth/auth.reducers";
import { IAuthState } from "../../state/auth/authState.interface";

@Component({
    selector: 'rw-editor',
    templateUrl: './editor.component.html',
})
export class EditorComponent {
    editorForm = this.fb.group({
        'title': ['', Validators.required],
        'description': ['', Validators.required],
        'body': ['', Validators.required],
        'tag': [''],
    })
    tagList: string[] = []
    isFormSubmitting$ = this.store.select(state => selectorIsEditorFormSubmitting(state as { [homeFeatureKey]: IHomeState}))
    //we can select from different feature state
    isHavingValidationErrors$ = this.store.select(state => selectorHaveValidationErrors(state as { [authFeatureKey]: IAuthState}))
    editorFormErrors$ = this.store.select(state => selectorValidationErrors(state as { [authFeatureKey]: IAuthState}))

    constructor(private fb: FormBuilder, private store: Store) {}

    addTag() {
        let tagToAdd = this.editorForm.value.tag

        if (tagToAdd === '' || tagToAdd === null || tagToAdd === undefined) {
            return
        }

        if (this.tagList.includes(tagToAdd)) {
            return
        }

        this.tagList.push(tagToAdd)
        this.editorForm.patchValue({ tag: '' })
    }

    //use tag name as unique identifier, use name to track changes
    trackByFn(index: number, tag: string) {
        return tag
    }

    removeTag(idx: number) {
        this.tagList.splice(idx, 1)
    }

    submitForm() {
        let toCreateArticleReq: IArticle = {
            title: this.editorForm.value.title!,
            description: this.editorForm.value.description!,
            body: this.editorForm.value.body!,
            tagList: this.tagList,
            slug: null,
            createdAt: null,
            updatedAt: null,
            favorited: null,
            author: null,
            favoritesCount: null,
        }
        console.log(toCreateArticleReq)
        this.store.dispatch(editorActions.submitEditorForm({ article: toCreateArticleReq }))
    }

}
