import { Routes } from '@angular/router';
import { ProductListComponent } from './product-list/product-list.component';
import { ProductFormModalComponent } from './product-form/product-form-modal.component';

export const PRODUCT_ROUTES: Routes = [
  { path: '', component: ProductListComponent, runGuardsAndResolvers: 'always' },
  { path: 'new', component: ProductFormModalComponent }
];
