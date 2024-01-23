import { HttpClient } from '@angular/common/http'
import { Injectable } from '@angular/core'
import { Observable, map } from 'rxjs'
import { environment as env } from '../../../environments/environment.development'

@Injectable()
export class TagService {
  constructor(private http: HttpClient) {}

  getTags(): Observable<string[]> {
    return this.http.get<{ tags: string[] }>(`${env.API_BASE_URL}/tags`).pipe(
      map((response: { tags: string[] }) => response.tags),
    )
  }
}
