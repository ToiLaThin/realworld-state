import { Component, OnDestroy, OnInit } from '@angular/core'
import { Store } from '@ngrx/store'
import { FormBuilder, Validators } from '@angular/forms'
import { Observable, Subscription } from 'rxjs'
import { IUser } from '../../core/models/user.interface'
import { authFeatureKey } from '../../state/auth/auth.reducers'
import { IAuthState } from '../../state/auth/authState.interface'
import { selectorCurrentUser } from '../../state/auth/auth.selectors'
import { ISettingsRequest } from '../types/settingsRequest.interface'
import { logoutActions, settingsActions } from '../../state/auth/auth.actions'

@Component({
  selector: 'rw-settings',
  templateUrl: './settings.component.html',
})
export class SettingsComponent implements OnInit, OnDestroy {
  settingsForm = this.fb.group({
    email: [''],
    bio: [''],
    image: [''],
    password: [''],
    username: [''],
  })
  subcriptionCurrUser!: Subscription
  currUser$!: Observable<IUser | null>

  constructor(private store: Store, private fb: FormBuilder) {}
  ngOnDestroy(): void {
    this.subcriptionCurrUser.unsubscribe()
  }

  ngOnInit(): void {
    this.currUser$ = this.store.select(state =>
      selectorCurrentUser(state as { [authFeatureKey]: IAuthState })
    )
    this.subcriptionCurrUser = this.currUser$.subscribe(currUser => {
      if (!currUser) {
        return
      }
      this.settingsForm.patchValue(currUser)
    })
  }

  logout() {
    this.store.dispatch(logoutActions.logout())
  }

  updateSettings() {
    console.log(this.settingsForm.value as ISettingsRequest)
    this.store.dispatch(
      settingsActions.updateSettings({
        settingsRequest: this.settingsForm.value as ISettingsRequest,
      })
    )
  }
}
