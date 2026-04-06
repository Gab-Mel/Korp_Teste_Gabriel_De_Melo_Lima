import { Component, EventEmitter, Output, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ProductService } from '../../../services/product.service';
import { InvoiceService } from '../../../services/invoice.service';
import { Invoice, InvoiceItem } from '../../../models/invoice.model';
import { Product } from '../../../models/product.model';

@Component({
  selector: 'app-invoice-form-modal',
  templateUrl: './invoice-form-modal.component.html',
  imports: [FormsModule, CommonModule],
  standalone: true
})
export class InvoiceFormModalComponent {
  @Output() close = new EventEmitter<void>();
  @Output() created = new EventEmitter<any>();
  @Output() printed = new EventEmitter<void>();

  products = signal<Product[]>([]);
  items = signal<InvoiceItem[]>([]);

  newInvoice = signal<Invoice | null>(null);
  loadingClose = signal(false);

  selectedProductId = 0;
  quantity = 1;
  customerName = '';


  private productService = inject(ProductService);
  private invoiceService = inject(InvoiceService);

  constructor() {
    this.productService.getAll().subscribe(data => {
      this.products.set(data);
    });
  }

  addItem() {
    if (!this.selectedProductId || this.quantity <= 0) return;
    this.items.set([...this.items(), { productId: this.selectedProductId, quantity: this.quantity }]);
  }

  updateItemQuantity(index: number, qty: number) {
    if (qty <= 0) return;
    const updatedItems = [...this.items()];
    updatedItems[index].quantity = qty;
    this.items.set(updatedItems);
  }

  save() {
    this.invoiceService.create({customerName: this.customerName, items: this.items() }).subscribe(newInvoice => {
      this.created.emit(newInvoice);
    });
  }

  saveAndPrint() {
    this.invoiceService.create({customerName: this.customerName, items: this.items() }).subscribe(newInvoice => {
      this.newInvoice.set(newInvoice);
      this.loadingClose.set(true);

      this.invoiceService.close(newInvoice.id).subscribe({
        next: () => {
          newInvoice.status = 'CLOSED';
          this.created.emit(newInvoice);
          this.loadingClose.set(false);
          this.close.emit();
          this.printed.emit();
          },
        error: () => {
          this.loadingClose.set(false);
          this.close.emit();
          this.printed.emit();
          alert('Erro ao fechar nota para impressão. A nota foi criada, mas não foi possível fechá-la automaticamente. Por favor, feche a nota manualmente para imprimir.');
    }});
    });
  }

  cancel() {
    this.close.emit();
  }
}
