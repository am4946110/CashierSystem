using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CashierSystem.Models;

public partial class CashierSystemContext : DbContext
{
    public CashierSystemContext()
    {
    }

    public CashierSystemContext(DbContextOptions<CashierSystemContext> options)
        : base(options)
    {
    }


  

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<PaymentType> PaymentTypes { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Sale> Sales { get; set; }

    public virtual DbSet<SaleDetail> SaleDetails { get; set; }

    public virtual DbSet<StockTransaction> StockTransactions { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<VwCurrentStock> VwCurrentStocks { get; set; }

    public virtual DbSet<VwCustomerSummary> VwCustomerSummaries { get; set; }

    public virtual DbSet<VwPayment> VwPayments { get; set; }

    public virtual DbSet<VwSalesDetail> VwSalesDetails { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=first");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<StockTransaction>()
          .Property(t => t.TransactionType)
          .HasConversion<string>();  // saves enum

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Categori__19093A0B127515B6");

            entity.ToTable("Categories", "ch");

            entity.HasIndex(e => e.CategoryName, "UQ__Categori__8517B2E00C85B6CF").IsUnique();

            entity.Property(e => e.CategoryId).ValueGeneratedNever();
            entity.Property(e => e.CategoryName).HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(250);
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__Customer__A4AE64D8426A2908");

            entity.ToTable("Customers", "ch");

            entity.Property(e => e.CustomerId).ValueGeneratedNever();
            entity.Property(e => e.Address).HasMaxLength(250);
            entity.Property(e => e.DateBecameCustomer).HasDefaultValueSql("(CONVERT([date],getdate()))");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(20);
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__Payments__9B556A380890EECF");

            entity.ToTable("Payments", "ch");

            entity.Property(e => e.PaymentId).ValueGeneratedNever();
            entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.PaidAt).HasDefaultValueSql("(sysdatetime())");

            entity.HasOne(d => d.PaymentType).WithMany(p => p.Payments)
                .HasForeignKey(d => d.PaymentTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Payments__Paymen__6477ECF3");

            entity.HasOne(d => d.Sale).WithMany(p => p.Payments)
                .HasForeignKey(d => d.SaleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Payments__SaleId__6383C8BA");
        });

        modelBuilder.Entity<PaymentType>(entity =>
        {
            entity.HasKey(e => e.PaymentTypeId).HasName("PK__PaymentT__BA430B353BBDE437");

            entity.ToTable("PaymentTypes", "ch");

            entity.HasIndex(e => e.TypeName, "UQ__PaymentT__D4E7DFA8C3A7CD20").IsUnique();

            entity.Property(e => e.PaymentTypeId).ValueGeneratedNever();
            entity.Property(e => e.TypeName).HasMaxLength(50);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Products__B40CC6CD762AE916");

            entity.ToTable("Products", "ch");

            entity.HasIndex(e => e.Barcode, "UQ__Products__177800D3FD70FB3B").IsUnique();

            entity.Property(e => e.ProductId).ValueGeneratedNever();
            entity.Property(e => e.Barcode).HasMaxLength(50);
            entity.Property(e => e.CostPrice).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.ProductName).HasMaxLength(100);
            entity.Property(e => e.ReorderLevel).HasDefaultValue(5);
            entity.Property(e => e.SalePrice).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Products__Catego__4AB81AF0");

            entity.HasOne(d => d.Supplier).WithMany(p => p.Products)
                .HasForeignKey(d => d.SupplierId)
                .HasConstraintName("FK__Products__Suppli__4BAC3F29");
        });

        modelBuilder.Entity<Sale>(entity =>
        {
            entity.HasKey(e => e.SaleId).HasName("PK__Sales__1EE3C3FFED5D86DA");

            entity.ToTable("Sales", "ch");

            entity.Property(e => e.SaleId).ValueGeneratedNever();
            entity.Property(e => e.Discount)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(10, 2)");
            entity.Property(e => e.NetAmount)
                .HasComputedColumnSql("([TotalAmount]-[Discount])", true)
                .HasColumnType("decimal(10, 2)");
            entity.Property(e => e.SaleDate).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Customer).WithMany(p => p.Sales)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__Sales__CustomerI__59063A47");
        });

        modelBuilder.Entity<SaleDetail>(entity =>
        {
            entity.HasKey(e => e.SaleDetailId).HasName("PK__SaleDeta__70DB14FE95BC2AAD");

            entity.ToTable("SaleDetails", "ch");

            entity.Property(e => e.SaleDetailId).ValueGeneratedNever();
            entity.Property(e => e.Total).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Product).WithMany(p => p.SaleDetails)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SaleDetai__Produ__5CD6CB2B");

            entity.HasOne(d => d.Sale).WithMany(p => p.SaleDetails)
                .HasForeignKey(d => d.SaleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SaleDetai__SaleI__5BE2A6F2");
        });

        modelBuilder.Entity<StockTransaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("PK__StockTra__55433A6B1FFFF521");

            entity.ToTable("StockTransactions", "ch");

            entity.Property(e => e.TransactionId).ValueGeneratedNever();
            entity.Property(e => e.Reference).HasMaxLength(100);
            entity.Property(e => e.TransactionDate).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.TransactionType).HasMaxLength(20);

            entity.HasOne(d => d.Product).WithMany(p => p.StockTransactions)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StockTran__Produ__534D60F1");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.SupplierId).HasName("PK__Supplier__4BE666B46539F8B7");

            entity.ToTable("Suppliers", "ch");

            entity.Property(e => e.SupplierId).ValueGeneratedNever();
            entity.Property(e => e.Address).HasMaxLength(250);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.SupplierName).HasMaxLength(100);
        });

        modelBuilder.Entity<VwCurrentStock>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_CurrentStock", "ch");

            entity.Property(e => e.Barcode).HasMaxLength(50);
            entity.Property(e => e.CategoryName).HasMaxLength(100);
            entity.Property(e => e.ProductName).HasMaxLength(100);
            entity.Property(e => e.SupplierName).HasMaxLength(100);
        });

        modelBuilder.Entity<VwCustomerSummary>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_CustomerSummary", "ch");

            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.TotalSpent).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<VwPayment>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_Payments", "ch");

            entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.CustomerName).HasMaxLength(100);
            entity.Property(e => e.NetAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.PaymentType).HasMaxLength(50);
        });

        modelBuilder.Entity<VwSalesDetail>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_SalesDetails", "ch");

            entity.Property(e => e.CustomerName).HasMaxLength(100);
            entity.Property(e => e.Discount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.LineTotal).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.NetAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ProductName).HasMaxLength(100);
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(10, 2)");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
