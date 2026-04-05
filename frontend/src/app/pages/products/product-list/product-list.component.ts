import { Component, OnInit, inject, signal } from '@angular/core';
import { ProductService } from '../../../services/product.service';
import { Product } from '../../../models/product.model';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ProductFormModalComponent } from '../product-form/product-form-modal.component';

@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html',
  imports: [FormsModule, CommonModule, ProductFormModalComponent],
  standalone: true
})
export class ProductListComponent implements OnInit {
  products = signal<Product[]>([]);
  loading = signal(true);
  showModal = signal(false);

  private service = inject(ProductService);

  ngOnInit() {
    this.load();
    this.service.refresh.subscribe(() => this.load());
  }

  load() {
    this.loading.set(true);
    this.service.getAll().subscribe({
      next: data => this.products.set(data),
      error: () => this.loading.set(false),
      complete: () => this.loading.set(false)
    });
  }

  addStock(product: Product) {
    const qty = Number(prompt('Quantidade a adicionar:'));
    if (!qty || qty <= 0) return;

    const updated = { ...product, quantity: product.quantity + qty };
    this.service.update(product.id, updated).subscribe(() => {
      // Atualiza o signal localmente, sem reload
      this.products.update(list =>
        list.map(p => (p.id === updated.id ? updated : p))
      );
    });
  }

  openModal() {
    this.showModal.set(true);
  }

  closeModal() {
    this.showModal.set(false);
  }

  onProductCreated(newProduct: Product) {
    // Adiciona o novo produto **reativamente** no signal
    this.products.update(list => [...list, newProduct]);
    this.closeModal();
  }

  trackById(index: number, item: Product) {
    return item.id; // ID único de cada produto
  }
  }
