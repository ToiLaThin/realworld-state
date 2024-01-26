import { ActivatedRouteSnapshot, Resolve, Router } from '@angular/router'
import { Observable, catchError } from 'rxjs'
import { ProfileService } from '../core/services/profile.service'
import { Injectable } from '@angular/core'
import { IProfile } from '../core/models/profile.interface'

@Injectable({
  providedIn: 'root',
})
export class UserNameProfileResolver implements Resolve<IProfile> {
  constructor(private profileService: ProfileService, private router: Router) {}

  resolve(route: ActivatedRouteSnapshot): Observable<any> {
    return this.profileService
      .getProfileOfUsername(route.params['username'])
      .pipe(catchError(err => this.router.navigateByUrl('/')))
  }
}
