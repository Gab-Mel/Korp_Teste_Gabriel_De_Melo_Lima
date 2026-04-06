-- Seed para Billing (Invoices + InvoiceItems)


-- Seed para Inventory (Products)
INSERT INTO "Products" ("Code", "Description", "Quantity", "Price", "Unit") VALUES
('P001', 'Caneta Azul', 100, 2.5, 'un'),
('P002', 'Caderno 100 folhas', 50, 15.0, 'un'),
('P003', 'Mochila Escolar', 20, 120.0, 'un'),
('P004', 'Lápis HB', 200, 1.5, 'un'),
('P005', 'Borracha', 150, 1.0, 'un');

-- Invoices
INSERT INTO "Invoices" ("Number", "CustomerName", "Status", "CreatedAt", "Total", "IdempotencyKey") VALUES
(1, 'João Silva', 'OPEN', NOW(), 25.0, 'seed-001'),
(2, 'Maria Santos', 'OPEN', NOW(), 30.0, 'seed-002'),
(3, 'Carlos Pereira', 'OPEN', NOW(), 145.0, 'seed-003');

-- Invoice Items
INSERT INTO "InvoiceItems" ("InvoiceId", "ProductId", "Description", "UnitPrice", "Quantity", "Total") VALUES
(1, 1, 'Caneta Azul', 2.5, 10, 25.0),
(2, 2, 'Caderno 100 folhas', 15.0, 2, 30.0),
(3, 3, 'Mochila Escolar', 120.0, 1, 120.0),
(3, 4, 'Lápis HB', 1.5, 5, 7.5),
(3, 5, 'Borracha', 1.0, 5, 5.0);