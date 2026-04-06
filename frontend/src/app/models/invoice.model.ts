export interface InvoiceItem {
  productId: number;
  quantity: number;
}

export interface InvoiceItemDetail {
  productId: number;
  quantity: number;
  descripition: string;
  price: number;
  unit: string;
  total: number;
}

export interface Invoice {
  id: number;
  number: number;
  customerName: string;
  status: string;
  total: number;
  createdAt: string;
  items: any[];
}
