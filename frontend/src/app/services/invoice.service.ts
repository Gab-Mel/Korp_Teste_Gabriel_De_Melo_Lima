import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { Observable, Subject } from 'rxjs';
import { InvoiceItem, Invoice } from '../models/invoice.model';

@Injectable({ providedIn: 'root' })
export class InvoiceService {
  private baseUrl = `${environment.api}/invoice`;
  private refresh$ = new Subject<void>();

  constructor(private http: HttpClient) {}

  get refresh() {
    return this.refresh$;
  }

  getAll() : Observable<Invoice[]> {
    return this.http.get<Invoice[]>(this.baseUrl);
  }

  create(invoice: {customerName: string, items: InvoiceItem[]}) : Observable<Invoice> {
    const key = Date.now() + '-' + Math.random();

    return this.http.post<Invoice>(this.baseUrl, invoice, {
      headers: {
        'Idempotency-Key': key
      }
    });
  }

  update(id: number, invoice: {items: InvoiceItem[]}) : Observable<Invoice> {
    return this.http.put<Invoice>(`${this.baseUrl}/${id}`, invoice);
  }

  close(id: number) {
    return this.http.put<void>(`${this.baseUrl}/${id}/close`, {});
  }

  delete(id: number) {
    return this.http.delete(`${this.baseUrl}/${id}`);
  }

  downloadPdf(id: number) {
    return this.http.get(`${this.baseUrl}/${id}/pdf`, { responseType: 'blob' });
  }
}
