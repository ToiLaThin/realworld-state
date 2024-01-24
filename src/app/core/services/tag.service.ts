import { HttpClient } from '@angular/common/http'
import { Injectable } from '@angular/core'
import { Observable, map } from 'rxjs'
import { environment as env } from '../../../environments/environment.development'

//since state is register in root
//the service must also registered in root
//since effect of state depends on this service

//if state is register by feature
//then  service can be registered by module
@Injectable({
  providedIn: 'root',
})
export class TagService {
  constructor(private http: HttpClient) {}

  getTags(): Observable<string[]> {
    return this.http.get<{ tags: string[] }>(`${env.API_BASE_URL}/tags`).pipe(
      map((response: { tags: string[] }) => response.tags),
    )
  }
}
