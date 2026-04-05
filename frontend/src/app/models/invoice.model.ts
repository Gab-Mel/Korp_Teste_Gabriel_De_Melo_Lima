export interface InvoiceItem {
  productId: number;
  quantity: number;
}

export interface Invoice {
  id: number;
  number: number;
  custumerName: string;
  status: string;
  createdAt: string;
  items: InvoiceItem[];
}
