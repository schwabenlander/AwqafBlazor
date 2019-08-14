using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using AwqafBlazor.Shared;

namespace AwqafBlazor.Server
{
    public partial class AwqafDbContext : DbContext
    {
        public AwqafDbContext(DbContextOptions<AwqafDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<AccountLedger> AccountsLedgers { get; set; }
        public virtual DbSet<FiscalYear> FiscalYears { get; set; }
        public virtual DbSet<Voucher> Vouchers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=AWQAF_TEST;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(e => e.AccountId)
                    .HasName("PK__ACCOUNTS__05B22F6020C446FE");

                entity.ToTable("ACCOUNTS");

                entity.Property(e => e.AccountId)
                    .HasColumnName("ACCOUNT_ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AccountName)
                    .IsRequired()
                    .HasColumnName("ACCOUNT")
                    .HasMaxLength(100);

                entity.Property(e => e.Level1).HasColumnName("LEVEL_1");

                entity.Property(e => e.Level2).HasColumnName("LEVEL_2");

                entity.Property(e => e.Level3).HasColumnName("LEVEL_3");

                entity.Property(e => e.Level4).HasColumnName("LEVEL_4");

                entity.Property(e => e.Remarks)
                    .HasColumnName("REMARKS")
                    .HasMaxLength(300);

                //entity.Property(e => e.SystemDate)
                //    .HasColumnName("SYSTEM_DATE")
                //    .HasColumnType("smalldatetime")
                //    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UserId).HasColumnName("USER_ID");
            });

            modelBuilder.Entity<AccountLedger>(entity =>
            {
                entity.HasKey(e => new { e.FiscalYearId, e.AccountId, e.LedgerNo })
                    .HasName("PK__ACCOUNTS__59EC7E4636F137B9");

                entity.ToTable("ACCOUNTS_LEDGERS");

                entity.Property(e => e.FiscalYearId).HasColumnName("FISCAL_YEAR_ID");

                entity.Property(e => e.AccountId).HasColumnName("ACCOUNT_ID");

                entity.Property(e => e.LedgerNo).HasColumnName("LEDGER_NO");

                entity.Property(e => e.Ledger)
                    .IsRequired()
                    .HasColumnName("LEDGER")
                    .HasMaxLength(250);

                entity.Property(e => e.Remarks)
                    .HasColumnName("REMARKS")
                    .HasMaxLength(300);

                //entity.Property(e => e.SystemDate)
                //    .HasColumnName("SYSTEM_DATE")
                //    .HasColumnType("smalldatetime")
                //    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UserId).HasColumnName("USER_ID");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.AccountLedgers)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK2_ACCOUNTS_LEDGERS");

                entity.HasOne(d => d.FiscalYear)
                    .WithMany(p => p.AccountLedgers)
                    .HasForeignKey(d => d.FiscalYearId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK1_ACCOUNTS_LEDGERS");
            });

            modelBuilder.Entity<FiscalYear>(entity =>
            {
                entity.HasKey(e => e.FiscalYearId)
                    .HasName("PK__FISCAL_Y__EBDB2745FA8F48D4");

                entity.ToTable("FISCAL_YEARS");

                entity.Property(e => e.FiscalYearId).HasColumnName("FISCAL_YEAR_ID");

                entity.Property(e => e.EndDate)
                    .HasColumnName("END_DATE")
                    .HasColumnType("smalldatetime");

                entity.Property(e => e.IsCurrent)
                    .HasColumnName("IS_CURRENT")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.IsOpen)
                    .HasColumnName("IS_OPEN")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.StartDate)
                    .HasColumnName("START_DATE")
                    .HasColumnType("smalldatetime");

                entity.Property(e => e.YearDescription)
                    .IsRequired()
                    .HasColumnName("YEAR_DESCRIPTION")
                    .HasMaxLength(4);
            });

            modelBuilder.Entity<Voucher>(entity =>
            {
                entity.HasKey(e => e.VoucherId)
                    .HasName("PK__VOUCHERS__60E7A0B33B050293");

                entity.ToTable("VOUCHERS");

                entity.Property(e => e.VoucherId)
                    .HasColumnName("VOUCHER_ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AccountId).HasColumnName("ACCOUNT_ID");

                entity.Property(e => e.FiscalYearId).HasColumnName("FISCAL_YEAR_ID");

                entity.Property(e => e.LedgerNo).HasColumnName("LEDGER_NO");

                entity.Property(e => e.Remarks)
                    .HasColumnName("REMARKS")
                    .HasMaxLength(1000);

                //entity.Property(e => e.SystemDate)
                //    .HasColumnName("SYSTEM_DATE")
                //    .HasColumnType("smalldatetime")
                //    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UserId).HasColumnName("USER_ID");

                entity.Property(e => e.VoucherDate)
                    .HasColumnName("VOUCHER_DATE")
                    .HasColumnType("smalldatetime");

                entity.Property(e => e.VoucherStatusId)
                    .HasColumnName("VOUCHER_STATUS_ID")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.VoucherTotal)
                    .HasColumnName("VOUCHER_TOTAL")
                    .HasColumnType("money")
                    .HasDefaultValueSql("((0))");

                entity.HasOne(d => d.AccountLedger)
                    .WithMany(p => p.Vouchers)
                    .HasForeignKey(d => new { d.FiscalYearId, d.AccountId, d.LedgerNo })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK1_VOUCHERS");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
