import { Routes } from '@angular/router';
import { ProductListComponent } from './pages/products/product-list/product-list.component';
import { InvoiceListComponent } from './pages/invoices/invoice-list/invoice-list.component';

export const routes: Routes = [
  { path: 'products', component: ProductListComponent },
  { path: 'invoices', component: InvoiceListComponent },
  { path: '', redirectTo: 'products', pathMatch: 'full' }
];
