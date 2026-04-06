import { Component, EventEmitter, Output, Input, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { InvoiceService } from '../../../services/invoice.service';
import { ProductService } from '../../../services/product.service';
import { Invoice, InvoiceItem } from '../../../models/invoice.model';
import { Product } from '../../../models/product.model';

@Component({
  selector: 'app-invoice-detail-modal',
  templateUrl: './invoice-detail-modal.component.html',
  imports: [FormsModule, CommonModule],
  standalone: true
})
export class InvoiceDetailModalComponent {
  @Input() invoice!: Invoice; // recebe a invoice externa
  @Output() close = new EventEmitter<void>();
  @Output() updated = new EventEmitter<Invoice>();
  @Output() deleted = new EventEmitter<number>(); // id da invoice deletada


  products = signal<Product[]>([]);

  selectedProductId = 0;
  quantity = 1;

  private invoiceService = inject(InvoiceService);
  private productService = inject(ProductService);

  onLoad = signal(true);

  constructor() {
    this.productService.getAll().subscribe(data => {
      this.products.set(data);
    });
  }

  addItem() {
    if (!this.invoice || this.invoice.status !== 'OPEN') return;
    if (!this.selectedProductId || this.quantity <= 0) return;

    const newItem: InvoiceItem = {
      productId: this.selectedProductId,
      quantity: this.quantity
    };


    this.invoice.items.push(newItem);
    this.selectedProductId = 0;
    this.quantity = 1;
  }

  updateItemQuantity(index: number, qty: number) {
    if (!this.invoice || this.invoice.status !== 'OPEN') return;
    if (qty <= 0) return;

    this.invoice.items[index].quantity = qty;
  }

  save() {
    if (!this.invoice) return;

    this.invoiceService.update(this.invoice.id, this.invoice).subscribe({
      next: updatedInvoice => {
        this.updated.emit(updatedInvoice);
        this.close.emit();
      },
      error: err => alert(err.message || 'Erro ao salvar invoice')
    });
  }

  print() {
    if (!this.invoice || this.invoice.status !== 'OPEN') return;

    this.invoiceService.close(this.invoice.id).subscribe({
      next: () => {
        this.invoice.status = 'CLOSED';
        this.updated.emit(this.invoice);
      },
      error: err => alert(err.message || 'Erro ao fechar invoice')
    });
  }

  delete() {
    if (!this.invoice || this.invoice.status !== 'OPEN') return;
    this.onLoad.set(true);

    this.invoiceService.delete(this.invoice.id).subscribe({
      next: () => {
        this.deleted.emit(this.invoice.id);
        this.close.emit();
        this.onLoad.set(false);
      },
      error: err => alert(err.message || 'Erro ao deletar invoice')
    });
  }

  cancel() {
    this.close.emit();
  }

  getProduct(item: InvoiceItem) {
    return this.products().find(p => p.id === item.productId);
  }
}
