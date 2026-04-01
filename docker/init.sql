-- PRODUCTS
CREATE TABLE products (
    id SERIAL PRIMARY KEY,
    code VARCHAR(50) UNIQUE, -- opcional, mas geralmente útil
    description TEXT NOT NULL,
    quantity INTEGER NOT NULL CHECK (quantity >= 0)
);

-- INVOICES
CREATE TABLE invoices (
    id SERIAL PRIMARY KEY,
    number INTEGER NOT NULL UNIQUE, -- numeração sequencial
    status VARCHAR(10) NOT NULL CHECK (status IN ('OPEN', 'CLOSED')),
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- INVOICE ITEMS (tabela intermediária)
CREATE TABLE invoice_items (
    id SERIAL PRIMARY KEY,
    invoice_id INTEGER NOT NULL,
    product_id INTEGER NOT NULL,
    quantity INTEGER NOT NULL CHECK (quantity > 0),

    CONSTRAINT fk_invoice
        FOREIGN KEY (invoice_id)
        REFERENCES invoices(id)
        ON DELETE CASCADE,

    CONSTRAINT fk_product
        FOREIGN KEY (product_id)
        REFERENCES products(id),

    CONSTRAINT unique_invoice_product
        UNIQUE (invoice_id, product_id)
);