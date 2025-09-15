CREATE VIEW ch.vw_SalesDetails
AS
SELECT 
    s.SaleId,
    s.SaleDate,
    c.FullName AS CustomerName,
    p.ProductName,
    sd.Quantity,
    sd.UnitPrice,
    sd.Total AS LineTotal,
    s.TotalAmount,
    s.Discount,
    s.NetAmount
FROM ch.Sales s
LEFT JOIN ch.Customers c ON s.CustomerId = c.CustomerId
INNER JOIN ch.SaleDetails sd ON s.SaleId = sd.SaleId
INNER JOIN ch.Products p ON sd.ProductId = p.ProductId;
 
 go

CREATE VIEW ch.vw_CurrentStock
AS
SELECT 
    p.ProductId,
    p.ProductName,
    p.Barcode,
    c.CategoryName,
    s.SupplierName,
    ISNULL(SUM(CASE WHEN st.TransactionType = 'IN' THEN st.Quantity ELSE 0 END),0) -
    ISNULL(SUM(CASE WHEN st.TransactionType = 'OUT' THEN st.Quantity ELSE 0 END),0) AS CurrentStock,
    p.ReorderLevel
FROM ch.Products p
INNER JOIN ch.Categories c ON p.CategoryId = c.CategoryId
LEFT JOIN ch.Suppliers s ON p.SupplierId = s.SupplierId
LEFT JOIN ch.StockTransactions st ON p.ProductId = st.ProductId
GROUP BY p.ProductId, p.ProductName, p.Barcode, c.CategoryName, s.SupplierName, p.ReorderLevel;

go

CREATE VIEW ch.vw_Payments
AS
SELECT 
    pay.PaymentId,
    pay.PaidAt,
    s.SaleId,
    c.FullName AS CustomerName,
    pt.TypeName AS PaymentType,
    pay.Amount,
    s.NetAmount
FROM ch.Payments pay
INNER JOIN ch.Sales s ON pay.SaleId = s.SaleId
LEFT JOIN ch.Customers c ON s.CustomerId = c.CustomerId
INNER JOIN ch.PaymentTypes pt ON pay.PaymentTypeId = pt.PaymentTypeId;

go

CREATE VIEW ch.vw_CustomerSummary
AS
SELECT 
    c.CustomerId,
    c.FullName,
    COUNT(s.SaleId) AS TotalOrders,
    ISNULL(SUM(s.NetAmount),0) AS TotalSpent,
    MIN(s.SaleDate) AS FirstPurchase,
    MAX(s.SaleDate) AS LastPurchase
FROM ch.Customers c
LEFT JOIN ch.Sales s ON c.CustomerId = s.CustomerId
GROUP BY c.CustomerId, c.FullName;
