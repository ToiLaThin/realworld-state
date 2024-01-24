import { HttpClient } from '@angular/common/http'
import { Injectable } from '@angular/core'
import { ISettingsRequest } from '../../settings/types/settingsRequest.interface'
import { environment as env } from '../../../environments/environment'
import { IUser } from '../models/user.interface'
import { Observable, map } from 'rxjs'

//since state is register in root
//the service must also registered in root
//since effect of state depends on this service

//if state is register by feature
//then  service can be registered by module

@Injectable({ providedIn: 'root' })
export class SettingsService {
  constructor(private http: HttpClient) {}

  updateSettings(settingsRequest: ISettingsRequest): Observable<IUser> {
    console.log('settingsRequest', settingsRequest)
    return this.http
      .put<{ user: IUser }>(`${env.API_BASE_URL}/user`, { user: settingsRequest })
      .pipe(map((response: { user: IUser }) => response.user))
  }
}
