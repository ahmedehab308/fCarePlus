import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AccountSearchDto } from '../../shared/models/journal.models';
import { environment } from '../../../environments/environment';
@Injectable({
  providedIn: 'root',
})
export class AccountService {

  private readonly baseUrl = environment.apiBaseUrl+'/Accounts';

  constructor(private http: HttpClient) {}


  searchAccounts(query: string): Observable<AccountSearchDto[]> {
    const params = new HttpParams().set('query', query);

    return this.http.get<AccountSearchDto[]>(`${this.baseUrl}/search`, { params });
  }

}
