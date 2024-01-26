import { HttpClient } from '@angular/common/http'
import { Injectable } from '@angular/core'
import { environment as env } from '../../../environments/environment.development'
import { Observable, map } from 'rxjs'
import { IProfile } from '../models/profile.interface'
@Injectable({
  providedIn: 'root',
})
export class ProfileService {
  constructor(private http: HttpClient) {}

  getProfileOfUsername(username: string): Observable<IProfile> {
    return this.http
      .get<{ profile: IProfile }>(`${env.API_BASE_URL}/profiles/${username}`)
      .pipe(map(response => response.profile))
    //response will be an object like: {profile: IProfile}    
  }
}
