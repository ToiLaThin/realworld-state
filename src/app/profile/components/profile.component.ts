import { Component, OnDestroy, OnInit } from '@angular/core'
import { IProfile } from '../../core/models/profile.interface'
import { Observable, Subscription, concatMap, map, of, tap } from 'rxjs'
import { ActivatedRoute, Router } from '@angular/router'
import { Store } from '@ngrx/store'
import { selectorCurrentUser, selectorIsLoggedIn } from '../../state/auth/auth.selectors'
import { authFeatureKey } from '../../state/auth/auth.reducers'
import { IAuthState } from './../../state/auth/authState.interface'
import { profileActions } from '../../state/profile/profile.actions'
import {
  selectorIsThisProfileOfCurrentUser,
  selectorViewingProfile,
  selectorIsFollowOrUnfollowProfileInProgress,
} from '../../state/profile/profile.selectors'
import { profileFeatureKey } from '../../state/profile/profile.reducers'
import { IProfileState } from '../../state/profile/profileState,interface'
import { homeActions } from '../../state/home/home.actions'
import { selectorIsOnProfileFavoritesTab } from '../../state/home/home.selectors'
import { IHomeState } from '../../state/home/homeState.interface'
import { homeFeatureKey } from '../../state/home/home.reducers'

@Component({
  selector: 'rw-profile',
  templateUrl: './profile.component.html',
})
export class ProfileComponent implements OnInit, OnDestroy {
  profileSelected$!: Observable<IProfile | null>
  isThisProfileOfCurrentUser$!: Observable<boolean>
  profileUsername!: string
  isOnFavoritedTab$!: Observable<boolean>
  isFollowOrUnfollowProfileInProgress$!: Observable<boolean>
  //   routeParamsSub!: Subscription
  isCurrentUserFollowingThisProfile!: boolean
  isUserAuthenticated!: boolean
  isUserAuthenticatedSubscription!: Subscription
  isCurrentUserFollowingThisProfileSubscription!: Subscription

  constructor(private route: ActivatedRoute, private store: Store, private router: Router) {}

  ngOnDestroy(): void {
    // this.routeParamsSub.unsubscribe()
    this.isCurrentUserFollowingThisProfileSubscription.unsubscribe()
    this.isUserAuthenticatedSubscription.unsubscribe()
  }

  ngOnInit() {
    // OPTION 1: use resolver to get data
    //why i cannot cast (data: { usernameProfile: IProfile })

    // use store to get Observable of current user then use operator map to whatever you want
    // let currentUser$ = this.store.select(state => selectorCurrentUser(state as {[authFeatureKey]: IAuthState}))
    // this.profileSelected$ = this.route.data.pipe(
    //   map((data): IProfile => data['usernameProfile'] as IProfile)
    // )

    // this.isThisProfileOfCurrentUser$ = this.route.data.pipe(
    //     concatMap((data) => currentUser$.pipe(
    //         map((currentUser) => {
    //             console.log(currentUser)
    //             console.log(data['usernameProfile'])
    //             return currentUser?.username === (data['usernameProfile'] as IProfile).username
    //         })
    //     ))
    // )

    // this.routeParamsSub = this.route.params.subscribe((params) => {
    //     console.log(params)
    //})

    // OPTION 2: use do not need to use resolver, only use store to get data, also dispatch filter article by author action
    // define a few more selector to get data from store, do not need to put data transform logic in component
    this.profileSelected$ = this.store.select(state =>
      selectorViewingProfile(state as { [profileFeatureKey]: IProfileState })
    )
    //see profile.selectors.ts to see how to make a selector aggregate to get data from multiple state, not just from multiple prop in a state
    this.isThisProfileOfCurrentUser$ = this.store.select(state =>
      selectorIsThisProfileOfCurrentUser(
        state as { [authFeatureKey]: IAuthState; [profileFeatureKey]: IProfileState }
      )
    )
    this.isOnFavoritedTab$ = this.store.select(state =>
      selectorIsOnProfileFavoritesTab(state as { [homeFeatureKey]: IHomeState })
    )
    this.isFollowOrUnfollowProfileInProgress$ = this.store.select(state =>
      selectorIsFollowOrUnfollowProfileInProgress(state as { [profileFeatureKey]: IProfileState })
    )
    this.profileUsername = this.route.snapshot.paramMap.get('username') as string
    this.store.dispatch(profileActions.profileOfUserSelected({ username: this.profileUsername }))
    //the article display will be of this author (profile), if filtered by tag or favorited, it will be reset to null

    this.store.dispatch(homeActions.filterByAuthorChanged({ author: this.profileUsername }))
    this.isCurrentUserFollowingThisProfileSubscription = this.profileSelected$.subscribe(
      profile => {
        if (profile) {
          this.isCurrentUserFollowingThisProfile = profile.following
        }
      }
    )
    this.isUserAuthenticatedSubscription = this.store
      .select(state => selectorIsLoggedIn(state as { [authFeatureKey]: IAuthState }))
      .subscribe(isAuthenticated => {
        if (isAuthenticated == null || isAuthenticated == undefined) {
          this.isUserAuthenticated = false
        }
        this.isUserAuthenticated = isAuthenticated as boolean
      })
  }

  viewProfileFavoritedArticles() {
    this.store.dispatch(homeActions.filterByFavoritedChanged({ favorited: this.profileUsername }))
  }

  viewProfileArticles() {
    this.store.dispatch(homeActions.filterByAuthorChanged({ author: this.profileUsername }))
  }

  unfollowProfile() {
    if (!this.isUserAuthenticated) {
      this.router.navigate(['/login'])
      return
    }
    this.store.dispatch(profileActions.unfollowProfile({ username: this.profileUsername }))
  }

  followProfile() {
    if (!this.isUserAuthenticated) {
      this.router.navigate(['/login'])
      return
    }
    this.store.dispatch(profileActions.followProfile({ username: this.profileUsername }))
  }
}
