import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@environments/environment';
import { get } from 'http';
import { Subject } from 'rxjs';
import { tap } from 'rxjs/operators';


@Injectable({ providedIn: 'root' })
export class ProductService {
  private baseUrl = `${environment.api}/product`;
  private refresh$ = new Subject<void>();

  constructor(private http: HttpClient) {}

  get refresh() {
    return this.refresh$;
  }

  getAll() {
    return this.http.get<any[]>(this.baseUrl);
  }

  create(product: any) {
    return this.http.post(this.baseUrl, product).pipe(
      tap(() => this.refresh$.next())
    );
  }

  update(id: number, product: any) {
    return this.http.put(`${this.baseUrl}/${id}`, product);
  }
}
