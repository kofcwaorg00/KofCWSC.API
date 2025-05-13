using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using KofCWSC.API.Models;
using Azure.Identity;
using Microsoft.Data.SqlClient.AlwaysEncrypted.AzureKeyVaultProvider;
using Microsoft.Data.SqlClient;
using Azure.Security.KeyVault.Secrets;
using Serilog;

namespace KofCWSC.API.Data
{
    public partial class KofCWSCAPIDBContext : DbContext
    {
        public KofCWSCAPIDBContext()
        {
        }
        private static bool isKVInit = false;
        public KofCWSCAPIDBContext(DbContextOptions<KofCWSCAPIDBContext> options)
            : base(options)
        {
            //*****************************************************************************************************************
            // 6/14/2024 Tim Philomeno
            // this is the magic code that allows client permissions for SQL Server to get its master encryption key from KeyVault
            // the authentication is done using DefaultAzureCredential.  That cycles through multple types
            // of authentication.  For the develoment environment, it uses the credentials that the developer has used to
            // login to Visual Studio.  For publised environments, you need to setup Azure Identity to allow
            // the applicaiton to authenticate and get access to KeyVault
            //*****************************************************************************************************************
            try
            {
                if (!isKVInit)
                {
                    SqlColumnEncryptionAzureKeyVaultProvider akvProvider = new SqlColumnEncryptionAzureKeyVaultProvider(new DefaultAzureCredential());
                    SqlConnection.RegisterColumnEncryptionKeyStoreProviders(customProviders: new Dictionary<string, SqlColumnEncryptionKeyStoreProvider>(capacity: 1, comparer: StringComparer.OrdinalIgnoreCase)
                    {
                        {
                            SqlColumnEncryptionAzureKeyVaultProvider.ProviderName, akvProvider
                        }
                    });
                    isKVInit = true;
                }

            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw new Exception("SQL Azure Key Vault Initialization Failed");
            }
        }

        public virtual DbSet<TblMasAward> TblMasAwards { get; set; }
        public virtual DbSet<TblCorrMemberOffice> TblCorrMemberOffices { get; set; }
        public virtual DbSet<TblValOffice> TblValOffices { get; set; }
        public virtual DbSet<MemberVM> funSYS_BuildName { get; set; }
        public virtual DbSet<SPFratYearVM> funSYS_GetBegFratYearN { get; set; }
        public virtual DbSet<NextID> uspWSC_GetNextTempID { get; set; }
        public virtual DbSet<TblValCouncil> TblValCouncils { get; set; }
        public virtual DbSet<TblValCouncilFSEdit> TblValCouncilsFSEdit { get; set; }
        public virtual DbSet<TblMasMember> TblMasMembers { get; set; } = null!;
        public virtual DbSet<GetLabelByOffice> GetLabelsByOffice { get; set; } = null!;
        public virtual DbSet<DirMain> DirMain { get; set; }
        public virtual DbSet<DirSupremeContacts> DirSupremeContacts { get; set; }
        public virtual DbSet<TblValAssy> TblValAssy { get; set; } = default!;
        public virtual DbSet<TblWebSelfPublish> TblWebSelfPublishes { get; set; }
        public virtual DbSet<KofCMemberIDUsers> KofCMemberIDUsers { get; set; }
        public virtual DbSet<TblMasPso> TblMasPsos { get; set; }
        public virtual DbSet<TblWebTrxAoi> TblWebTrxAois { get; set; }
        public virtual DbSet<CvnImpDelegatesLog> CvnImpDelegatesLogs { get; set; }
        public virtual DbSet<SPGetChairmenId> SPGetChairmanIDs { get; set; }
        public virtual DbSet<TblSysTrxEvents> TblSysTrxEvents { get; set; }
        public virtual DbSet<EmailOffice> TblWebTrxEmailOffices { get; set; }
        public virtual DbSet<FileStorage> FileStorages { get; set; }
        public virtual DbSet<CvnControl> TblCvnControls { get; set; }
        public virtual DbSet<RollCallSheets> RollCallSheets { get; set; }
        
        public virtual DbSet<CvnImpDelegatesLog> TblCvnImpDelegatesLogs { get; set; }
        public virtual DbSet<CvnDelegateDays> CvnDelegateDays { get; set; }
        public virtual DbSet<CvnMileage> TblCvnMasMileages { get; set; }
        public virtual DbSet<CvnMileageC> TblCvnMasMileagesC { get; set; }
        public virtual DbSet<CvnLocation> TblCvnMasLocations { get; set; }
        public virtual DbSet<CvnMpd> TblCvnTrxMpds { get; set; }
        public virtual DbSet<CvnImpDelegateIMP> CvnImpDelegateIMPs { get; set; }
        public virtual DbSet<MemberSuspension> TblSysMasMemberSuspensions { get; set; }
        public virtual DbSet<NecImpNecrology> TblNecImpNecrologies { get; set; }
        public virtual DbSet<LogCorrMemberOffice> TblLogCorrMemberOffices { get; set; }
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
            
            modelBuilder.Entity<MemberVM>(entity =>
            {
                entity.HasNoKey();
            });
            modelBuilder.Entity<SPFratYearVM>(entity =>
            {
                entity.HasNoKey();
            });
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

                entity.Property(e => e.KofCid).HasColumnName("KofCID");

                entity.Property(e => e.LastLoggedIn).HasColumnType("datetime");

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.Property(e => e.LastNameUpdated).HasColumnType("datetime");

                entity.Property(e => e.LastNameUpdatedBy).HasMaxLength(100);

                entity.Property(e => e.LastUpdated).HasColumnType("datetime");

                entity.Property(e => e.MI)
                    .HasMaxLength(50)
                    .HasColumnName("MI");

                entity.Property(e => e.MIUpdated)
                    .HasColumnType("datetime")
                    .HasColumnName("MIUpdated");

                entity.Property(e => e.MIUpdatedBy)
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

            modelBuilder.Entity<TblMasPso>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__tbl_MasP__3214EC27821953F8");

                entity.ToTable("tbl_MasPSOs");

                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.StateAdvocate)
                    .HasMaxLength(255)
                    .HasColumnName("State Advocate");
                entity.Property(e => e.StateDeputy)
                    .HasMaxLength(255)
                    .HasColumnName("State Deputy");
                entity.Property(e => e.StateSecretary)
                    .HasMaxLength(255)
                    .HasColumnName("State Secretary");
                entity.Property(e => e.StateTreasurer)
                    .HasMaxLength(255)
                    .HasColumnName("State Treasurer");
                entity.Property(e => e.StateWarden)
                    .HasMaxLength(255)
                    .HasColumnName("State Warden");
            });
            modelBuilder.Entity<TblWebTrxAoi>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.ToTable("tblWEB_TrxAOI");

                entity.Property(e => e.GraphicUrl)
                    .HasMaxLength(250)
                    .HasColumnName("GraphicURL");
                entity.Property(e => e.LinkUrl)
                    .HasMaxLength(250)
                    .HasColumnName("LinkURL");
                entity.Property(e => e.PostedDate).HasColumnType("datetime");
                entity.Property(e => e.Title).HasMaxLength(250);
                entity.Property(e => e.Type)
                    .HasMaxLength(2)
                    .IsFixedLength();
            });
            modelBuilder.Entity<TblValAssy>(entity =>
            {
                entity.HasKey(e => e.ANumber)
                    .HasName("aaaaatbl_ValAssys_PK")
                    .IsClustered(false);

                entity.ToTable("tbl_ValAssys");

                entity.Property(e => e.ANumber)
                    .ValueGeneratedNever()
                    .HasColumnName("A_NUMBER");
                entity.Property(e => e.ALocation)
                    .HasMaxLength(50)
                    .HasColumnName("A_LOCATION");
                entity.Property(e => e.AName)
                    .HasMaxLength(50)
                    .HasColumnName("A_NAME");
                entity.Property(e => e.AddInfo1)
                    .HasMaxLength(60)
                    .HasColumnName("ADD INFO 1");
                entity.Property(e => e.AddInfo2)
                    .HasMaxLength(60)
                    .HasColumnName("ADD INFO 2");
                entity.Property(e => e.AddInfo3)
                    .HasMaxLength(60)
                    .HasColumnName("ADD INFO 3");
                entity.Property(e => e.MasterLoc)
                    .HasMaxLength(1)
                    .IsFixedLength();
                entity.Property(e => e.WebSiteUrl).HasColumnName("WebSiteURL");
            });

            modelBuilder.Entity<GetLabelByOffice>(entity =>
            {
                entity.HasNoKey();
            });
            modelBuilder.Entity<DirMain>(entity =>
            {
                entity.HasNoKey();
            });
            modelBuilder.Entity<DirSupremeContacts>(entity =>
            {
                entity.HasNoKey();
            });
            modelBuilder.Entity<TblValCouncilFSEdit>(entity =>
            {
                entity.HasKey(e => e.CNumber);
            });

            modelBuilder.Entity<TblValCouncil>(entity =>
            {
                entity.HasKey(e => e.CNumber)
                    .HasName("aaaaatbl_ValCouncils_PK")
                    .IsClustered(false);

                entity.ToTable("tbl_ValCouncils");

                entity.Property(e => e.CNumber)
                    .ValueGeneratedNever()
                    .HasColumnName("C_NUMBER");
                entity.Property(e => e.AddInfo1)
                    .HasMaxLength(60)
                    .HasColumnName("ADD INFO 1");
                entity.Property(e => e.AddInfo2)
                    .HasMaxLength(60)
                    .HasColumnName("ADD INFO 2");
                entity.Property(e => e.AddInfo3)
                    .HasMaxLength(50)
                    .HasColumnName("ADD INFO 3");
                entity.Property(e => e.Arbalance)
                    .HasColumnType("numeric(14, 2)")
                    .HasColumnName("ARBalance");
                entity.Property(e => e.BulletinUrl).HasColumnName("BulletinURL");
                entity.Property(e => e.CLocation)
                    .HasMaxLength(50)
                    .HasColumnName("C_LOCATION");
                entity.Property(e => e.CName)
                    .HasMaxLength(32)
                    .HasColumnName("C_NAME");
                entity.Property(e => e.Chartered).HasColumnType("datetime");
                entity.Property(e => e.DioceseId)
                    .HasMaxLength(3)
                    .HasColumnName("DioceseID");
                entity.Property(e => e.District).HasColumnName("DISTRICT");
                entity.Property(e => e.LiabIns).HasDefaultValue(false);
                entity.Property(e => e.Status)
                    .HasMaxLength(1)
                    .HasDefaultValue("A")
                    .IsFixedLength();
                entity.Property(e => e.WebSiteUrl).HasColumnName("WebSiteURL");

                entity.Property(e => e.PhyAddress)
                    .HasMaxLength(255)
                    .HasColumnName("PhyAddress");
                entity.Property(e => e.PhyCity)
                    .HasMaxLength(50)
                    .HasColumnName("PhyCity");
                entity.Property(e => e.PhyState)
                    .HasMaxLength(20)
                    .HasColumnName("PhyState");
                entity.Property(e => e.PhyPostalCode)
                    .HasMaxLength(20)
                    .HasColumnName("PhyPostalCode");

                entity.Property(e => e.MailAddress)
                    .HasMaxLength(255)
                    .HasColumnName("MailAddress");
                entity.Property(e => e.MailCity)
                   .HasMaxLength(50)
                   .HasColumnName("MailCity");
                entity.Property(e => e.MailState)
                    .HasMaxLength(20)
                    .HasColumnName("MailState");
                entity.Property(e => e.MailPostalCode)
                    .HasMaxLength(20)
                    .HasColumnName("MailPostalCode");

                entity.Property(e => e.MeetAddress)
                    .HasMaxLength(255)
                    .HasColumnName("MeetAddress");
                entity.Property(e => e.MeetCity)
                   .HasMaxLength(50)
                   .HasColumnName("MeetCity");
                entity.Property(e => e.MeetState)
                    .HasMaxLength(20)
                    .HasColumnName("MeetState");
                entity.Property(e => e.MeetPostalCode)
                    .HasMaxLength(20)
                    .HasColumnName("MeetPostalCode");
                entity.Property(e => e.Updated)
                    .HasColumnName("Updated");
                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("UpdatedBy");

            });

            modelBuilder.Entity<TblValOffice>(entity =>
            {
                entity.HasKey(e => e.OfficeId)
                    .HasName("aaaaatbl_ValOffices_PK")
                    .IsClustered(false);

                entity.ToTable("tbl_ValOffices", tb =>
                {
                    tb.HasTrigger("T_tbl_ValOffices_DTrig");
                    tb.HasTrigger("T_tbl_ValOffices_UTrig");
                });

                entity.Property(e => e.OfficeId).HasColumnName("OfficeID");
                entity.Property(e => e.AltDescription).HasMaxLength(75);
                entity.Property(e => e.EmailAlias)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.OfficeDescription).HasMaxLength(75);
                entity.Property(e => e.SupremeUrl).HasColumnName("SupremeURL");
                entity.Property(e => e.UseAsFormalTitle).HasDefaultValue(false);
            });

            modelBuilder.Entity<TblWebSelfPublish>(entity =>
            {
                entity.HasKey(e => e.Url).HasName("PK_tblWEB_SelfPublish_1");

                entity.ToTable("tblWEB_SelfPublish");

                entity.Property(e => e.Url)
                    .HasMaxLength(400)
                    .HasColumnName("URL");
                entity.Property(e => e.Data)
                    .HasColumnType("text");
                entity.Property(e => e.OID)
                    .HasColumnType("int")
                    .HasColumnName("OID");
            });

            modelBuilder.Entity<TblMasAward>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__tbl_MasA__3214EC078DBBE52E");

                entity.ToTable("tbl_MasAwards");

                entity.Property(e => e.AwardDescription)
                    .HasMaxLength(255)
                    .HasColumnName("Award Description");
                entity.Property(e => e.AwardDueDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Award Due Date");
                entity.Property(e => e.AwardName)
                    .HasMaxLength(255)
                    .HasColumnName("Award Name");
                entity.Property(e => e.AwardSubmissionEmailAddress)
                    .HasMaxLength(255)
                    .HasColumnName("Award Submission Email Address");
                entity.Property(e => e.LinkToTheAwardForm)
                    .HasMaxLength(255)
                    .HasColumnName("Link to the Award Form");
            });

            modelBuilder.Entity<TblWebSelfPublish>(entity =>
            {
                entity.HasKey(e => e.Url).HasName("PK_tblWEB_SelfPublish_1");

                entity.ToTable("tblWEB_SelfPublish");

                entity.Property(e => e.Url)
                    .HasMaxLength(400)
                    .HasColumnName("URL");
                entity.Property(e => e.Data)
                    .HasColumnType("text");
                entity.Property(e => e.OID)
                    .HasColumnType("int")
                    .HasColumnName("OID");
            });
            modelBuilder.Entity<SPGetChairmenId>(entity =>
            {
                entity.Property(e => e.Id);
            });
            modelBuilder.Entity<TblCorrMemberOffice>(entity =>
            {
                entity.ToTable(tb => tb.HasTrigger("trgAfterChangeCMO"));
                entity.ToTable("tbl_CorrMemberOffice");

                modelBuilder.Entity<SPGetSOS>(entity =>
                {
                    entity.HasNoKey();
                });

            });

            modelBuilder.Entity<TblSysTrxEvents>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__tblSYS_t__3214EC07638F8109");

                entity.ToTable("tblSYS_trxEvents");

                entity.Property(e => e.AddedBy).HasMaxLength(50);
                entity.Property(e => e.AttachUrl)
                    .HasMaxLength(250)
                    .HasColumnName("AttachURL");
                entity.Property(e => e.Begin).HasColumnType("datetime");
                entity.Property(e => e.DateAdded).HasColumnType("datetime");
                entity.Property(e => e.End).HasColumnType("datetime");
                entity.Property(e => e.Title).HasMaxLength(50);
                entity.Property(e => e.isAllDay).HasColumnType("boolean");
            });
            modelBuilder.Entity<EmailOffice>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_tblWEB_EmailOffice");

                entity.ToTable("tblWEB_trxEmailOffice");

                entity.Property(e => e.Dd).HasColumnName("DD");
                entity.Property(e => e.Fc).HasColumnName("FC");
                entity.Property(e => e.Fn).HasColumnName("FN");
                entity.Property(e => e.From).HasMaxLength(50);
                entity.Property(e => e.Fs).HasColumnName("FS");
                entity.Property(e => e.Gk).HasColumnName("GK");
                entity.Property(e => e.DateSent).HasColumnName("DateSent");
                entity.Property(e => e.Subject).HasMaxLength(50);
            });
            modelBuilder.Entity<FileStorage>(
           dob =>
           {
               dob.ToTable("tblWEB_FileStorage");
           });
            modelBuilder.Entity<NextID>(entity =>
            {
                entity.HasNoKey();
            });
            modelBuilder.Entity<CvnControl>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_tblCVN_Control1");

                entity.ToTable("tblCVN_Control");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.LocationString).HasMaxLength(1000);
            });
            modelBuilder.Entity<RollCallSheets>(entity =>
            {
                entity.HasNoKey();
            });
            modelBuilder.Entity<CvnImpDelegateIMP>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");
                entity.ToTable("tblCVN_ImpDelegates");

                entity.Property(e => e.A1Address1)
                    .HasMaxLength(255)
                    .HasColumnName("A1Address1");
                entity.Property(e => e.A1Address2)
                    .HasMaxLength(255)
                    .HasColumnName("A1Address2");
                entity.Property(e => e.A1City)
                    .HasMaxLength(255)
                    .HasColumnName("A1City");
                entity.Property(e => e.A1Email)
                    .HasMaxLength(255)
                    .HasColumnName("A1Email");
                entity.Property(e => e.A1FirstName)
                    .HasMaxLength(255)
                    .HasColumnName("A1FirstName");
                entity.Property(e => e.A1LastName)
                    .HasMaxLength(255)
                    .HasColumnName("A1LastName");
                entity.Property(e => e.A1MemberID).HasColumnName("A1MemberID");
                entity.Property(e => e.A1MiddleName)
                    .HasMaxLength(255)
                    .HasColumnName("A1MiddleName");
                entity.Property(e => e.A1Phone)
                    .HasMaxLength(255)
                    .HasColumnName("A1Phone");
                entity.Property(e => e.A1State)
                    .HasMaxLength(255)
                    .HasColumnName("A1State");
                entity.Property(e => e.A1Suffix)
                    .HasMaxLength(255)
                    .HasColumnName("A1Suffix");
                entity.Property(e => e.A1ZipCode)
                    .HasMaxLength(255)
                    .HasColumnName("A1ZipCode");
                entity.Property(e => e.A2Address1)
                    .HasMaxLength(255)
                    .HasColumnName("A2Address1");
                entity.Property(e => e.A2Address2)
                    .HasMaxLength(255)
                    .HasColumnName("A2Address2");
                entity.Property(e => e.A2City)
                    .HasMaxLength(255)
                    .HasColumnName("A2City");
                entity.Property(e => e.A2Email)
                    .HasMaxLength(255)
                    .HasColumnName("A2Email");
                entity.Property(e => e.A2FirstName)
                    .HasMaxLength(255)
                    .HasColumnName("A2FirstName");
                entity.Property(e => e.A2LastName)
                    .HasMaxLength(255)
                    .HasColumnName("A2LastName");
                entity.Property(e => e.A2MemberID).HasColumnName("A2MemberID");
                entity.Property(e => e.A2MiddleName)
                    .HasMaxLength(255)
                    .HasColumnName("A2MiddleName");
                entity.Property(e => e.A2Phone)
                    .HasMaxLength(255)
                    .HasColumnName("A2Phone");
                entity.Property(e => e.A2State)
                    .HasMaxLength(255)
                    .HasColumnName("A2State");
                entity.Property(e => e.A2Suffix)
                    .HasMaxLength(255)
                    .HasColumnName("A2Suffix");
                entity.Property(e => e.A2ZipCode)
                    .HasMaxLength(255)
                    .HasColumnName("A2ZipCode");
                entity.Property(e => e.CouncilName)
                    .HasMaxLength(255)
                    .HasColumnName("CouncilName");
                entity.Property(e => e.CouncilNumber).HasColumnName("CouncilNumber");
                entity.Property(e => e.D1Address1)
                    .HasMaxLength(255)
                    .HasColumnName("D1Address1");
                entity.Property(e => e.D1Address2)
                    .HasMaxLength(255)
                    .HasColumnName("D1Address2");
                entity.Property(e => e.D1City)
                    .HasMaxLength(255)
                    .HasColumnName("D1City");
                entity.Property(e => e.D1Email)
                    .HasMaxLength(255)
                    .HasColumnName("D1Email");
                entity.Property(e => e.D1FirstName)
                    .HasMaxLength(255)
                    .HasColumnName("D1FirstName");
                entity.Property(e => e.D1LastName)
                    .HasMaxLength(255)
                    .HasColumnName("D1LastName");
                entity.Property(e => e.D1MemberID).HasColumnName("D1MemberID");
                entity.Property(e => e.D1MiddleName)
                    .HasMaxLength(255)
                    .HasColumnName("D1MiddleName");
                entity.Property(e => e.D1Phone)
                    .HasMaxLength(255)
                    .HasColumnName("D1Phone");
                entity.Property(e => e.D1State)
                    .HasMaxLength(255)
                    .HasColumnName("D1State");
                entity.Property(e => e.D1Suffix)
                    .HasMaxLength(255)
                    .HasColumnName("D1Suffix");
                entity.Property(e => e.D1ZipCode)
                    .HasMaxLength(255)
                    .HasColumnName("D1ZipCode");
                entity.Property(e => e.D2Address1)
                    .HasMaxLength(255)
                    .HasColumnName("D2Address1");
                entity.Property(e => e.D2Address2)
                    .HasMaxLength(255)
                    .HasColumnName("D2Address2");
                entity.Property(e => e.D2City)
                    .HasMaxLength(255)
                    .HasColumnName("D2City");
                entity.Property(e => e.D2Email)
                    .HasMaxLength(255)
                    .HasColumnName("D2Email");
                entity.Property(e => e.D2FirstName)
                    .HasMaxLength(255)
                    .HasColumnName("D2FirstName");
                entity.Property(e => e.D2LastName)
                    .HasMaxLength(255)
                    .HasColumnName("D2LastName");
                entity.Property(e => e.D2MemberID).HasColumnName("D2MemberID");
                entity.Property(e => e.D2MiddleName)
                    .HasMaxLength(255)
                    .HasColumnName("D2MiddleName");
                entity.Property(e => e.D2Phone)
                    .HasMaxLength(255)
                    .HasColumnName("D2Phone");
                entity.Property(e => e.D2State)
                    .HasMaxLength(255)
                    .HasColumnName("D2State");
                entity.Property(e => e.D2Suffix)
                    .HasMaxLength(255)
                    .HasColumnName("D2Suffix");
                entity.Property(e => e.D2ZipCode)
                    .HasMaxLength(255)
                    .HasColumnName("D2ZipCode");
                entity.Property(e => e.FormSubmitterSEmail)
                    .HasMaxLength(255)
                    .HasColumnName("FormSubmitterSEmail");
                
                entity.Property(e => e.SubmissionDate)
                    .HasColumnType("datetime")
                    .HasColumnName("SubmissionDate");
            });
            modelBuilder.Entity<CvnImpDelegatesLog>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.ToTable("tblCVN_ImpDelegatesLog");

                entity.Property(e => e.Data).IsUnicode(false);
                entity.Property(e => e.MemberId).HasColumnName("MemberID");
                entity.Property(e => e.Rundate).HasColumnType("datetime");
                entity.Property(e => e.Type)
                    .HasMaxLength(10)
                    .IsUnicode(false);
                entity.Property(e => e.Guid).HasColumnName("GUID");
            });
            modelBuilder.Entity<CvnMileage>(entity =>
            {
                entity.ToTable("tblCVN_MasMileage");

                entity.Property(e => e.Location).HasMaxLength(50);
            });
            modelBuilder.Entity<CvnLocation>(entity =>
            {
                entity.ToTable("tblCVN_MasLocations");

                entity.HasIndex(e => e.Location, "IX_tblCVN_MasLocations").IsUnique();

                entity.Property(e => e.Location).HasMaxLength(50);
            });
            modelBuilder.Entity<CvnMpd>(entity =>
            {
                entity.ToTable("tblCVN_TrxMPD");

                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.CheckTotal).HasColumnType("numeric(10, 2)");
                entity.Property(e => e.Day1GD1).HasMaxLength(50);
                entity.Property(e => e.Day2GD1).HasMaxLength(50);
                entity.Property(e => e.Day3GD1).HasMaxLength(50);
                entity.Property(e => e.Day1GD2).HasMaxLength(50);
                entity.Property(e => e.Day2GD2).HasMaxLength(50);
                entity.Property(e => e.Day3GD2).HasMaxLength(50);
                entity.Property(e => e.Group).HasMaxLength(50);
                entity.Property(e => e.Location).HasMaxLength(50);
                entity.Property(e => e.MemberId).HasColumnName("MemberID");
                entity.Property(e => e.Office).HasMaxLength(50);
                entity.Property(e => e.Payee).HasMaxLength(50);
            });

            modelBuilder.Entity<MemberSuspension>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__tblSYS_M__3214EC07A5FFC31C");

                entity.ToTable("tblSYS_MasMemberSuspension");

                entity.Property(e => e.KofCid).HasColumnName("KofCId");
                entity.Property(e => e.Comment).HasColumnName("Comment");
                entity.Property(e => e.Updated).HasColumnName("Updated");
                entity.Property(e => e.UpdatedBy).HasColumnName("UpdatedBy");
            });

            modelBuilder.Entity<CvnDelegateDays>(entity =>
            {
                entity.HasNoKey();
            });
            modelBuilder.Entity<NecImpNecrology>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToTable("tblNEC_ImpNecrology");

                entity.Property(e => e.AssemblyId).HasColumnName("AssemblyID");
                entity.Property(e => e.Comments).IsUnicode(false);
                entity.Property(e => e.CouncilId).HasColumnName("CouncilID");
                entity.Property(e => e.DecFmorKn)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("DecFMorKN");
                entity.Property(e => e.DecFname)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("DecFName");
                entity.Property(e => e.DecLname)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("DecLName");
                entity.Property(e => e.DecMemberId).HasColumnName("DecMemberID");
                entity.Property(e => e.DecMname)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("DecMName");
                entity.Property(e => e.DecOfficesHeld)
                    .HasMaxLength(500)
                    .IsUnicode(false);
                entity.Property(e => e.DecPrefix)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.DecSuffix)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Dod)
                    .HasColumnType("datetime")
                    .HasColumnName("DOD");
                entity.Property(e => e.Fmfname)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("FMFName");
                entity.Property(e => e.Fmlname)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("FMLName");
                entity.Property(e => e.Fmmname)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("FMMName");
                entity.Property(e => e.Fmprefix)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("FMPrefix");
                entity.Property(e => e.Fmsuffix)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("FMSuffix");
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.HasKey(e => e.Id);
                entity.Property(e => e.MemberType)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Nokaddress1)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("NOKAddress1");
                entity.Property(e => e.Nokaddress2)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("NOKAddress2");
                entity.Property(e => e.Nokcity)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("NOKCity");
                entity.Property(e => e.Nokcountry)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("NOKCountry");
                entity.Property(e => e.Nokfname)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("NOKFName");
                entity.Property(e => e.Noklname)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("NOKLname");
                entity.Property(e => e.Nokmname)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("NOKMName");
                entity.Property(e => e.Nokprefix)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("NOKPrefix");
                entity.Property(e => e.Nokrelate)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("NOKRelate");
                entity.Property(e => e.Nokstate)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("NOKState");
                entity.Property(e => e.Noksuffix)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("NOKSuffix");
                entity.Property(e => e.Nokzip)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("NOKZip");
                entity.Property(e => e.Relationship)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.SubDate).HasColumnType("datetime");
                entity.Property(e => e.SubEmail)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.SubFname)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("SubFName");
                entity.Property(e => e.SubLname)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("SubLName");
                entity.Property(e => e.SubRole)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });
            modelBuilder.Entity<LogCorrMemberOffice>(entity =>
            {
                entity.ToTable("tblLOG_CorrMemberOffice");

                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.ChangeDate).HasColumnType("datetime");
                entity.Property(e => e.ChangeType).HasMaxLength(10);
                entity.Property(e => e.MemberId).HasColumnName("MemberID");
                entity.Property(e => e.OfficeId).HasColumnName("OfficeID");
            });

            OnModelCreatingPartial(modelBuilder);
        }
       
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }



}
