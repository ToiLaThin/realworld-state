import { HttpClient } from '@angular/common/http'
import { ILoginRequest } from '../../auth/types/loginRequest.interface'
import { environment as env } from '../../../environments/environment.development'
import { Observable, map, tap } from 'rxjs'
import { IUser } from '../models/user.interface'
import { Injectable } from '@angular/core'
import { JwtService } from '../../shared/services/jwt.service'
import { IProfile } from '../models/profile.interface'
import { IRegisterRequest } from '../../auth/types/registerRequest.interface'

//since state is register in root
//the service must also registered in root
//since effect of state depends on this service

//if state is register by feature
//then  service can be registered by module
@Injectable({ providedIn: 'root' })
export class UserService {
  constructor(private http: HttpClient, private jwtService: JwtService) {}

  login(loginRequest: ILoginRequest): Observable<IUser> {
    return this.http
      .post<{ user: IUser }>(`${env.API_BASE_URL}/users/login`, { user: loginRequest })
      .pipe(
        //the returned data is an object with a user: IUser property
        map(data => data.user),
        //tap operator to do side effect, we do not want to modify the stream
        tap((returnedUser: IUser) => {
          console.log('returnedUser', returnedUser)
          console.log('returnedUser.token', returnedUser.token)
          this.jwtService.saveToken(returnedUser.token)
        })
      )
  }

  register(registerRequest: IRegisterRequest): Observable<IUser> {
    return this.http
      .post<{ user: IUser }>(`${env.API_BASE_URL}/users`, { user: registerRequest })
      .pipe(
        map(data => data.user),
        //tap operator to do side effect, we do not want to modify the stream
        tap((returnedUser: IUser) => {
          console.log('returnedUser', returnedUser)
          console.log('returnedUser.token', returnedUser.token)
          this.jwtService.saveToken(returnedUser.token)
        })
      )
  }

  followUser(username: string): Observable<IProfile> {
    return this.http
      .post<{ profile: IProfile }>(`${env.API_BASE_URL}/profiles/${username}/follow`, {})
      .pipe(map(resp => resp.profile))
  }

  unfollowUser(username: string): Observable<IProfile> {
    return this.http
      .delete<{ profile: IProfile }>(`${env.API_BASE_URL}/profiles/${username}/follow`, {})
      .pipe(map(resp => resp.profile))
  }
}
