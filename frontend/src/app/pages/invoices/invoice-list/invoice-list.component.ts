import { Component, OnInit, inject, signal } from '@angular/core';
import { InvoiceService } from '../../../services/invoice.service';
import { Invoice } from '../../../models/invoice.model';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { InvoiceFormModalComponent } from '../invoice-form/invoice-form-modal.component';
import { InvoiceDetailModalComponent } from '../invoice-detail/invoice-detail-modal.component';

@Component({
  selector: 'app-invoice-list',
  templateUrl: './invoice-list.component.html',
  imports: [FormsModule, CommonModule, InvoiceFormModalComponent, InvoiceDetailModalComponent],
  standalone: true
})
export class InvoiceListComponent implements OnInit {
  invoices = signal<Invoice[]>([]);
  loading = signal(true);
  showModal = signal(false);

  private service = inject(InvoiceService);

  ngOnInit() {
    this.load();
    this.service.refresh.subscribe(() => this.load());
  }

  load() {
    this.loading.set(true);
    this.service.getAll().subscribe({
      next: data => this.invoices.set(data),
      error: () => this.loading.set(false),
      complete: () => this.loading.set(false)
    });
  }

  openModal() {
    this.showModal.set(true);
  }

  closeModal() {
    this.showModal.set(false);
  }

  onInvoiceCreated(newInvoice: Invoice) {
    // Adiciona a invoice reativamente
    this.invoices.update(list => [...list, newInvoice]);
    this.closeModal();
  }

  print(id: number) {
    this.service.close(id).subscribe(() => {
      this.invoices.update(list =>
        list.map(inv => inv.id === id ? { ...inv, status: 'CLOSED' } : inv)
      );
    });
  }

  trackById(index: number, item: Invoice) {
    return item.id;
  }

  showDetailModal = signal(false);
  selectedInvoice: any;

  openDetailModal(inv: any) {
    this.selectedInvoice = inv;
    this.showDetailModal.set(true);
  }

  closeDetailModal() {
    this.showDetailModal.set(false);
    this.selectedInvoice = null;
  }

  onInvoiceUpdated(updatedInvoice: Invoice) {
    this.invoices.update(list =>
      list.map(inv => inv.id === updatedInvoice.id ? updatedInvoice : inv)
    );
    this.closeDetailModal();
  }

  onInvoicePrinted() {
    this.load();
  }

  onInvoiceDeleted(deletedInvoiceId: number) {
    this.invoices.update(list => list.filter(inv => inv.id !== deletedInvoiceId));
    this.closeDetailModal();
  }
}
