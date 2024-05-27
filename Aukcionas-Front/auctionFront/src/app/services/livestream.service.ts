import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class LivestreamService {
  private baseApiUrl: string = environment.apiUrl + '/Livestream';
  constructor(private http: HttpClient) {}

  startLivestream(auctionId: number): Observable<string> {
    return this.http.post<string>(
      `${this.baseApiUrl}/start/${auctionId}`,
      null
    );
  }

  stopLivestream(auctionId: number): Observable<void> {
    return this.http.post<void>(`${this.baseApiUrl}/stop/${auctionId}`, null);
  }
}
