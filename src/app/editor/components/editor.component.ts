import { Component, OnDestroy, OnInit } from "@angular/core";
import { FormBuilder, Validators } from "@angular/forms";
import { Store } from "@ngrx/store";
import { IArticle } from "../../core/models/article.interface";
import { editorActions } from "../../state/home/home.actions";
import { selectorIsEditorFormSubmitting } from "../../state/home/home.selectors";
import { homeFeatureKey } from "../../state/home/home.reducers";
import { IHomeState } from "../../state/home/homeState.interface";
import { selectorHaveValidationErrors, selectorValidationErrors } from "../../state/auth/auth.selectors";
import { authFeatureKey } from "../../state/auth/auth.reducers";
import { IAuthState } from "../../state/auth/authState.interface";
import { ActivatedRoute } from "@angular/router";
import { selectorSelectedArticle } from "../../state/article/article.selectors";
import { articleFeatureKey } from "../../state/article/article.reducers";
import { IArticleState } from "../../state/article/articleState.interface";
import { Subscription } from "rxjs";

@Component({
    selector: 'rw-editor',
    templateUrl: './editor.component.html',
})
export class EditorComponent implements OnInit, OnDestroy{
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


    selectedArticle$ = this.store.select(state => selectorSelectedArticle(state as { [articleFeatureKey]: IArticleState }))
    selectedArticleSubcription!: Subscription
    constructor(private fb: FormBuilder, private store: Store,private route: ActivatedRoute) {}

    ngOnDestroy(): void {
        this.selectedArticleSubcription.unsubscribe()
    }
    ngOnInit(): void {
        let selectedArticleSlug = this.route.snapshot.paramMap.get('slug') //meaning we render this to edit an existing article
        if (selectedArticleSlug === null || selectedArticleSlug === undefined) {
            return
        }
        this.selectedArticleSubcription = this.selectedArticle$.subscribe((selectedArticle: IArticle | null) => {
            if (selectedArticle === null) {
                return
            }
            this.editorForm.patchValue({
                title: selectedArticle.title,
                description: selectedArticle.description,
                body: selectedArticle.body,
            })
            //if we use this.tagList = selectedArticle.tagList, we will have error, find out why
            this.tagList = [...selectedArticle.tagList]
        })

    }

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
            slug: this.route.snapshot.paramMap.get('slug') as string, 
            createdAt: null,
            updatedAt: null,
            favorited: null,
            author: null,
            favoritesCount: null,
        }
        console.log(toCreateArticleReq)
        //the slug is null or not will be check in the effect
        this.store.dispatch(editorActions.submitEditorForm({ article: toCreateArticleReq }))

    }

}
