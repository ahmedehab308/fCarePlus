import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { JournalVoucherInputModel } from '../../shared/models/journal.models';
import { environment } from '../../../environments/environment';
@Injectable({
  providedIn: 'root',
})
export class JournalService {

  private readonly baseUrl = environment.apiBaseUrl+'/Journal';

  constructor(private http: HttpClient) {}


  saveVoucher(model: JournalVoucherInputModel): Observable<any> {
  return this.http.post<any>(`${this.baseUrl}/save`, model).pipe(
    catchError(error => throwError(() => new Error(error.error?.Message || 'Failed to save the voucher.')))
  );
}

}
