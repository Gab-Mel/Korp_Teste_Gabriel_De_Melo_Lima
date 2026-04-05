import { Component, EventEmitter, Output, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ProductService } from '../../../services/product.service';
import { Product } from '../../../models/product.model';

@Component({
  selector: 'app-product-form-modal',
  templateUrl: './product-form-modal.component.html',
  imports: [FormsModule, CommonModule],
  standalone: true
})
export class ProductFormModalComponent {
  @Output() close = new EventEmitter<void>();
  @Output() created = new EventEmitter<Product>();

  product: Product = { id: 0, code: '', description: '', quantity: 0, price: 0, unit: '' };
  private service = inject(ProductService);
  public units = ['unidades', 'kg', 'litros']; // Exemplo de unidades, pode ser ajustado conforme necessário

  save() {
    this.service.create(this.product).subscribe((newProd) => {
      this.created.emit(newProd as Product);
    });
  }

  cancel() {
    this.close.emit();
  }
}
