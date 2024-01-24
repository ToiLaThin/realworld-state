import {
  ActivatedRouteSnapshot,
  CanActivate,
  Router,
  RouterStateSnapshot,
  UrlTree,
} from '@angular/router'
import { Store } from '@ngrx/store'
import { Observable } from 'rxjs'
import { selectorIsLoggedIn } from '../../state/auth/auth.selectors'
import { authFeatureKey } from '../../state/auth/auth.reducers'
import { IAuthState } from '../../state/auth/authState.interface'
import { Injectable } from '@angular/core'

@Injectable({
  providedIn: 'root',
})
export class AuthGuard implements CanActivate {
  constructor(private store: Store, private router: Router) {}
  isAuthenticated$: Observable<boolean> = this.store.select(state => {
    const isLoggedIn = selectorIsLoggedIn(state as { [authFeatureKey]: IAuthState })
    if (isLoggedIn === null || isLoggedIn === false) {
      this.router.navigateByUrl('/login')
      return false
    }
    return isLoggedIn
  })
  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> {
    return this.isAuthenticated$
  }
}
