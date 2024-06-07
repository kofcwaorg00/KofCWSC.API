using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using KofCWSC.API.Models;

namespace KofCWSC.API.Data
{
    public partial class KofCWSCAPIDBContext : DbContext
    {
        public KofCWSCAPIDBContext()
        {
        }

        public KofCWSCAPIDBContext(DbContextOptions<KofCWSCAPIDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TblMasMember> TblMasMembers { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                //optionsBuilder.UseSqlServer("Data Source=tcp:sql2k805.discountasp.net;Initial Catalog=SQL2008R2_137411_kofcwsc;User ID=SQL2008R2_137411_kofcwsc_user;Password=S1995KC;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TblMasMember>(entity =>
            {
                entity.HasKey(e => e.MemberId);

                entity.ToTable("tbl_MasMembers", tb =>
                {
                    // this is because our table has triggers
                    tb.UseSqlOutputClause(false);

                    tb.HasTrigger("T_tbl_MasMembers_DTrig");
                    tb.HasTrigger("T_tbl_MasMembers_U1Trig");
                    tb.HasTrigger("T_tbl_MasMembers_UTrig");
                });

                entity.HasIndex(e => e.KofCid, "NonClusteredIndex-20130527-110545")
                    .IsUnique();

                entity.Property(e => e.MemberId).HasColumnName("MemberID");

                entity.Property(e => e.AddInfo1).HasMaxLength(100);

                entity.Property(e => e.AddInfo1Updated).HasColumnType("datetime");

                entity.Property(e => e.AddInfo1UpdatedBy).HasMaxLength(100);

                entity.Property(e => e.AddInfo2).HasMaxLength(100);

                entity.Property(e => e.AddInfo2Updated).HasColumnType("datetime");

                entity.Property(e => e.AddInfo2UpdatedBy).HasMaxLength(100);

                entity.Property(e => e.Address).HasMaxLength(255);

                entity.Property(e => e.AddressUpdated).HasColumnType("datetime");

                entity.Property(e => e.AddressUpdatedBy).HasMaxLength(100);

                entity.Property(e => e.AssemblyUpdated).HasColumnType("datetime");

                entity.Property(e => e.AssemblyUpdatedBy).HasMaxLength(100);

                entity.Property(e => e.Bulletin).HasDefaultValueSql("((0))");

                entity.Property(e => e.BulletinUpdated).HasColumnType("datetime");

                entity.Property(e => e.BulletinUpdatedBy).HasMaxLength(100);

                entity.Property(e => e.CanEditAdmUi).HasColumnName("CanEditAdmUI");

                entity.Property(e => e.CellPhone).HasMaxLength(50);

                entity.Property(e => e.CellPhoneUpdated).HasColumnType("datetime");

                entity.Property(e => e.CellPhoneUpdatedBy).HasMaxLength(100);

                entity.Property(e => e.CircleUpdated).HasColumnType("datetime");

                entity.Property(e => e.CircleUpdatedBy).HasMaxLength(100);

                entity.Property(e => e.City).HasMaxLength(50);

                entity.Property(e => e.CityUpdated).HasColumnType("datetime");

                entity.Property(e => e.CityUpdatedBy).HasMaxLength(100);

                entity.Property(e => e.CouncilUpdated).HasColumnType("datetime");

                entity.Property(e => e.CouncilUpdatedBy).HasMaxLength(100);

                entity.Property(e => e.Data)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Deceased).HasDefaultValueSql("((0))");

                entity.Property(e => e.DeceasedUpdated).HasColumnType("datetime");

                entity.Property(e => e.DeceasedUpdatedBy).HasMaxLength(100);

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.EmailUpdated).HasColumnType("datetime");

                entity.Property(e => e.EmailUpdatedBy).HasMaxLength(100);

                entity.Property(e => e.FaxNumber).HasMaxLength(30);

                entity.Property(e => e.FaxNumberUpdated).HasColumnType("datetime");

                entity.Property(e => e.FaxNumberUpdatedBy).HasMaxLength(100);

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.FirstNameUpdated).HasColumnType("datetime");

                entity.Property(e => e.FirstNameUpdatedBy).HasMaxLength(100);

                entity.Property(e => e.KofCid)
                    .HasMaxLength(7)
                    .HasColumnName("KofCID");

                entity.Property(e => e.LastLoggedIn).HasColumnType("datetime");

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.Property(e => e.LastNameUpdated).HasColumnType("datetime");

                entity.Property(e => e.LastNameUpdatedBy).HasMaxLength(100);

                entity.Property(e => e.LastUpdated).HasColumnType("datetime");

                entity.Property(e => e.Mi)
                    .HasMaxLength(50)
                    .HasColumnName("MI");

                entity.Property(e => e.Miupdated)
                    .HasColumnType("datetime")
                    .HasColumnName("MIUpdated");

                entity.Property(e => e.MiupdatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("MIUpdatedBy");

                entity.Property(e => e.NickName).HasMaxLength(50);

                entity.Property(e => e.NickNameUpdated).HasColumnType("datetime");

                entity.Property(e => e.NickNameUpdatedBy).HasMaxLength(100);

                entity.Property(e => e.PaidMpd)
                    .HasColumnName("PaidMPD")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Phone).HasMaxLength(30);

                entity.Property(e => e.PhoneUpdated).HasColumnType("datetime");

                entity.Property(e => e.PhoneUpdatedBy).HasMaxLength(100);

                entity.Property(e => e.PostalCode).HasMaxLength(20);

                entity.Property(e => e.PostalCodeUpdated).HasColumnType("datetime");

                entity.Property(e => e.PostalCodeUpdatedBy).HasMaxLength(100);

                entity.Property(e => e.Prefix).HasMaxLength(50);

                entity.Property(e => e.PrefixUpdated).HasColumnType("datetime");

                entity.Property(e => e.PrefixUpdatedBy).HasMaxLength(100);

                entity.Property(e => e.State).HasMaxLength(20);

                entity.Property(e => e.StateUpdated).HasColumnType("datetime");

                entity.Property(e => e.StateUpdatedBy).HasMaxLength(100);

                entity.Property(e => e.Suffix).HasMaxLength(10);

                entity.Property(e => e.SuffixUpdated).HasColumnType("datetime");

                entity.Property(e => e.SuffixUpdatedBy).HasMaxLength(10);

                entity.Property(e => e.UserId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("UserID");

                entity.Property(e => e.WifesName).HasMaxLength(12);

                entity.Property(e => e.WifesNameUpdated).HasColumnType("datetime");

                entity.Property(e => e.WifesNameUpdatedBy).HasMaxLength(100);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }



}
