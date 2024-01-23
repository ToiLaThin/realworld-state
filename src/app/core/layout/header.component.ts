import { Component } from '@angular/core'
import { Store } from '@ngrx/store'
import { selectorCurrentUser, selectorIsLoggedIn } from '../../auth/state/auth.selectors'
import { authFeatureKey } from '../../auth/state/auth.reducers'
import { IAuthState } from '../../auth/types/authState.interface'
import { Observable } from 'rxjs'
import { IUser } from '../models/user.interface'

@Component({
  selector: 'rw-layout-header',
  templateUrl: './header.component.html',
})
export class HeaderComponent {
  constructor(private store: Store) {}

  isLoggedIn$: Observable<boolean | null> = this.store.select((state)  => selectorIsLoggedIn(state as {[authFeatureKey]: IAuthState}))
  currUser$: Observable<IUser | null> = this.store.select((state)  => selectorCurrentUser(state as {[authFeatureKey]: IAuthState}))
}
