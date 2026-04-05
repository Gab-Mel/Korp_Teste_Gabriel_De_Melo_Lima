import { Routes } from '@angular/router';
import { InvoiceListComponent } from './invoice-list/invoice-list.component';
import { InvoiceFormModalComponent } from './invoice-form/invoice-form-modal.component';
import { InvoiceDetailModalComponent } from './invoice-detail/invoice-detail-modal.component';

export const INVOICE_ROUTES: Routes = [
  { path: '', component: InvoiceListComponent},
  { path: 'new', component: InvoiceFormModalComponent },
  { path: ':id', component: InvoiceDetailModalComponent }
];
