using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using fCarePlus.API.Data.Entities;

namespace fCarePlus.API.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AccountsChart> AccountsCharts { get; set; }

    public virtual DbSet<JournalDetail> JournalDetails { get; set; }

    public virtual DbSet<JournalHeader> JournalHeaders { get; set; }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //    => optionsBuilder.UseSqlServer("Name=ConnectionStrings:DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AccountsChart>(entity =>
        {
            entity.HasKey(e => e.Id)
                .HasName("PK__Accounts__3214EC2748817F42")
                .HasFillFactor(90);

            entity.ToTable("AccountsChart");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.AllowEntry).HasColumnName("Allow_Entry");
            entity.Property(e => e.BranchId).HasColumnName("Branch_ID");
            entity.Property(e => e.ChartLevelDepth).HasColumnName("Chart_Level_Depth");
            entity.Property(e => e.CreationDate)
                .HasColumnType("datetime")
                .HasColumnName("Creation_Date");
            entity.Property(e => e.FkCostCenterTypeId).HasColumnName("FK_Cost_Center_Type_ID");
            entity.Property(e => e.FkTransactionTypeId).HasColumnName("FK_Transaction_Type_ID");
            entity.Property(e => e.FkWorkFieldsId).HasColumnName("FK_Work_Fields_ID");
            entity.Property(e => e.IsActive).HasColumnName("Is_Active");
            entity.Property(e => e.NameAr)
                .HasMaxLength(150)
                .HasColumnName("NameAR");
            entity.Property(e => e.NameEn)
                .HasMaxLength(150)
                .HasColumnName("NameEN");
            entity.Property(e => e.NoOfChilds).HasColumnName("noOfChilds");
            entity.Property(e => e.Number)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.OrgId).HasColumnName("Org_ID");
            entity.Property(e => e.ParentId).HasColumnName("Parent_ID");
            entity.Property(e => e.ParentNumber)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Parent_Number");
            entity.Property(e => e.Status).HasDefaultValue(true);
            entity.Property(e => e.UserId).HasColumnName("User_ID");
        });

        modelBuilder.Entity<JournalDetail>(entity =>
        {
            entity.HasKey(e => e.DetailId).HasName("PK__JournalD__4730833182603741");

            entity.Property(e => e.DetailId).HasColumnName("Detail_ID");
            entity.Property(e => e.AccountId).HasColumnName("Account_ID");
            entity.Property(e => e.CreditAmount)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 4)")
                .HasColumnName("Credit_Amount");
            entity.Property(e => e.DebitAmount)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 4)")
                .HasColumnName("Debit_Amount");
            entity.Property(e => e.DetailStatement)
                .HasMaxLength(500)
                .HasColumnName("Detail_Statement");
            entity.Property(e => e.IsDeleted).HasColumnName("Is_Deleted");
            entity.Property(e => e.JournalId).HasColumnName("Journal_ID");

            entity.HasOne(d => d.Account).WithMany(p => p.JournalDetails)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__JournalDe__Accou__44FF419A");

            entity.HasOne(d => d.Journal).WithMany(p => p.JournalDetails)
                .HasForeignKey(d => d.JournalId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__JournalDe__Journ__440B1D61");
        });

        modelBuilder.Entity<JournalHeader>(entity =>
        {
            entity.HasKey(e => e.JournalId).HasName("PK__JournalH__8AF06F724F37B0AE");

            entity.ToTable("JournalHeader");

            entity.Property(e => e.JournalId).HasColumnName("Journal_ID");
            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Creation_Date");
            entity.Property(e => e.IsDeleted).HasColumnName("Is_Deleted");
            entity.Property(e => e.JournalDate).HasColumnName("Journal_Date");
            entity.Property(e => e.JournalDescription)
                .HasMaxLength(500)
                .HasColumnName("Journal_Description");
            entity.Property(e => e.TotalCredit)
                .HasColumnType("decimal(18, 4)")
                .HasColumnName("Total_Credit");
            entity.Property(e => e.TotalDebit)
                .HasColumnType("decimal(18, 4)")
                .HasColumnName("Total_Debit");
            entity.Property(e => e.UserId).HasColumnName("User_ID");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
