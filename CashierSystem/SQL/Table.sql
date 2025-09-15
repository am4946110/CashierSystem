CREATE DATABASE  CashierSystem
go
create schema ch
go
CREATE TABLE ch.Categories (
    CategoryId uniqueidentifier  PRIMARY KEY,
    CategoryName NVARCHAR(100) NOT NULL UNIQUE,
    Description NVARCHAR(250)
);

CREATE TABLE ch.Suppliers (
    SupplierId uniqueidentifier PRIMARY KEY,
    SupplierName NVARCHAR(100) NOT NULL,
    Phone NVARCHAR(20),
    Email NVARCHAR(100),
    Address NVARCHAR(250)
);

CREATE TABLE ch.Products (
    ProductId uniqueidentifier PRIMARY KEY,
    ProductName NVARCHAR(100) NOT NULL,
    CategoryId uniqueidentifier NOT NULL,
    SupplierId uniqueidentifier NULL,
    Barcode NVARCHAR(50) UNIQUE,
    CostPrice DECIMAL(10,2) NOT NULL,
    SalePrice DECIMAL(10,2) NOT NULL,
    ReorderLevel INT DEFAULT 5,
    IsActive BIT DEFAULT 1,
    CreatedAt DATETIME2 DEFAULT SYSDATETIME(),
    FOREIGN KEY (CategoryId) REFERENCES ch.Categories(CategoryId),
    FOREIGN KEY (SupplierId) REFERENCES ch.Suppliers(SupplierId)
);

CREATE TABLE ch.Customers (
    CustomerId uniqueidentifier PRIMARY KEY,
    FullName NVARCHAR(100) NOT NULL,
    Phone NVARCHAR(20),
    Email NVARCHAR(100),
    Address NVARCHAR(250),
    DateBecameCustomer DATE DEFAULT CAST(GETDATE() AS DATE)
);


CREATE TABLE ch.StockTransactions (
    TransactionId uniqueidentifier PRIMARY KEY,
    ProductId uniqueidentifier NOT NULL,
    Quantity INT NOT NULL,
    TransactionType NVARCHAR(20) CHECK (TransactionType IN ('IN', 'OUT')),
    TransactionDate DATETIME2 DEFAULT SYSDATETIME(),
    Reference NVARCHAR(100),
    FOREIGN KEY (ProductId) REFERENCES ch.Products(ProductId)
);


CREATE TABLE ch.Sales (
    SaleId uniqueidentifier PRIMARY KEY,
    CustomerId uniqueidentifier NULL,
    UserId uniqueidentifier NOT NULL,
    SaleDate DATETIME2 DEFAULT SYSDATETIME(),
    TotalAmount DECIMAL(10,2) NOT NULL DEFAULT 0,
    Discount DECIMAL(10,2) DEFAULT 0,
    NetAmount AS (TotalAmount - Discount) PERSISTED,
    FOREIGN KEY (CustomerId) REFERENCES ch.Customers(CustomerId),
);

CREATE TABLE ch.SaleDetails (
    SaleDetailId uniqueidentifier PRIMARY KEY,
    SaleId uniqueidentifier NOT NULL,
    ProductId uniqueidentifier NOT NULL,
    Quantity INT NOT NULL,
    UnitPrice DECIMAL(10,2) NOT NULL,
    Total DECIMAL(10,2) NOT NULL,
    FOREIGN KEY (SaleId) REFERENCES ch.Sales(SaleId),
    FOREIGN KEY (ProductId) REFERENCES ch.Products(ProductId)
);


CREATE TABLE ch.PaymentTypes (
    PaymentTypeId uniqueidentifier PRIMARY KEY,
    TypeName NVARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE ch.Payments (
    PaymentId uniqueidentifier PRIMARY KEY,
    SaleId uniqueidentifier NOT NULL,
    PaymentTypeId uniqueidentifier NOT NULL,
    Amount DECIMAL(10,2) NOT NULL,
    PaidAt DATETIME2 DEFAULT SYSDATETIME(),
    FOREIGN KEY (SaleId) REFERENCES ch.Sales(SaleId),
    FOREIGN KEY (PaymentTypeId) REFERENCES ch.PaymentTypes(PaymentTypeId)
);