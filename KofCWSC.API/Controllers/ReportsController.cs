using KofCWSC.API.Data;
using KofCWSC.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace KofCWSC.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly KofCWSCAPIDBContext _context;

        public ReportsController(KofCWSCAPIDBContext context)
        {
            Log.Information("Initialize DB Context");
            _context = context;
        }

        [HttpGet("/GetLabelByOffice/{OfficeID}")]
        public IEnumerable<GetLabelByOffice> GetLabelByOffice(int OfficeID)
        {
            Log.Information($"Starting GetLabelByOffice {OfficeID}");
            if (OfficeID == 0)
            {
                OfficeID = 1;
            }
            try
            {
                return _context.Database
                .SqlQuery<GetLabelByOffice>($"uspRPT_GetLabelByOfficeFR {OfficeID}")
                .ToList();

            }
            catch (Exception ex)
            {

                Log.Fatal(ex.Message + " " + ex.InnerException);
                var _new = new List<GetLabelByOffice>();
                _new = null;
                return _new;
            }
        }
        [HttpGet("/GetLabelByGroup/{GroupID}")]
        public IEnumerable<GetLabelByOffice> GetLabelByGroup(int GroupID)
        {
            Log.Information($"Starting GetLabelByGroup {GroupID}");
            if (GroupID == 0)
            {
                GroupID = 1;
            }
            try
            {
                return _context.Database
                .SqlQuery<GetLabelByOffice>($"uspRPT_GetLabelByGroupFR {GroupID}")
                .ToList();

            }
            catch (Exception ex)
            {

                Log.Fatal(ex.Message + " " + ex.InnerException);
                var _new = new List<GetLabelByOffice>();
                _new = null;
                return _new;
            }
        }

        [HttpGet("/GetDirMain/{ShortForm}/{NextYear}")]
        public IEnumerable<DirMain> GetDirMain(int ShortForm,int NextYear)
        {
            try
            {
                return _context.Database
                    .SqlQuery<DirMain>($"[uspRPT_DirectoryMain] {ShortForm},{NextYear}")
                    .ToList();
            }
            catch (Exception ex)
            {

                Log.Fatal(GetType() + " - " + ex.Message + " " + ex.InnerException);
                var _new = new List<DirMain>();
                _new = null;
                return _new;
            }
        }
        [HttpGet("/GetDirSupremeContacts/{ShortForm}/{NextYear}")]
        public IEnumerable<DirMain> GetDirSupremeContacts(int ShortForm,int NextYear)
        {
            try
            {
                return _context.Database
                    .SqlQuery<DirMain>($"[uspRPT_DirectorySupremeContacts] {ShortForm}, {NextYear}")
                    .ToList();
            }
            catch (Exception ex)
            {
                Log.Fatal(GetType() + " - " + ex.Message + " " + ex.InnerException);
                var _new = new List<DirMain>();
                _new = null;
                return _new;
            }
        }
        [HttpGet("/GetDirStateOfficers/{ShortForm}/{NextYear}")]
        public IEnumerable<DirMain> GetDirStateOfficers(int ShortForm,int NextYear)
        {
            try
            {
                return _context.Database
                    .SqlQuery<DirMain>($"[uspRPT_DirectoryStateOfficers] {ShortForm}, {NextYear}")
                    .ToList();
            }
            catch (Exception ex)
            {
                Log.Fatal(GetType() + " - " + ex.Message + " " + ex.InnerException);
                var _new = new List<DirMain>();
                _new = null;
                return _new;
            }
        }
        [HttpGet("/GetDirChancery/{ShortForm}/{NextYear}")]
        public IEnumerable<DirMain> GetDirChancery(int ShortForm,int NextYear)
        {
            try
            {
                return _context.Database
                    .SqlQuery<DirMain>($"[uspRPT_DirectoryChancery] {ShortForm},{NextYear}")
                    .ToList();
            }
            catch (Exception ex)
            {
                Log.Fatal(GetType() + " - " + ex.Message + " " + ex.InnerException);
                var _new = new List<DirMain>();
                _new = null;
                return _new;
            }
        }
        [HttpGet("/GetDirCCBoard/{ShortForm}/{NextYear}")]
        public IEnumerable<DirMain> GetDirCCBoard(int ShortForm,int NextYear)
        {
            try
            {
                return _context.Database
                    .SqlQuery<DirMain>($"[uspRPT_DirectoryCCBoard] {ShortForm},{NextYear}")
                    .ToList();
            }
            catch (Exception ex)
            {
                Log.Fatal(GetType() + " - " + ex.Message + " " + ex.InnerException);
                var _new = new List<DirMain>();
                _new = null;
                return _new;
            }
        }
        [HttpGet("/GetDirPFHBoard/{ShortForm}/{NextYear}")]
        public IEnumerable<DirMain> GetDirPFHBoard(int ShortForm,int NextYear)
        {
            try
            {
                return _context.Database
                    .SqlQuery<DirMain>($"[uspRPT_DirectoryPFHBoard] {ShortForm},{NextYear}")
                    .ToList();
            }
            catch (Exception ex)
            {
                Log.Fatal(GetType() + " - " + ex.Message + " " + ex.InnerException);
                var _new = new List<DirMain>();
                _new = null;
                return _new;
            }
        }
        [HttpGet("/GetDirDDs/{ShortForm}/{NextYear}")]
        public IEnumerable<DirMain> GetDirDDs(int ShortForm,int NextYear)
        {
            try
            {
                return _context.Database
                    .SqlQuery<DirMain>($"[uspRPT_DirectoryDDs] {ShortForm},{NextYear}")
                    .ToList();
            }
            catch (Exception ex)
            {
                Log.Fatal(GetType() + " - " + ex.Message + " " + ex.InnerException);
                var _new = new List<DirMain>();
                _new = null;
                return _new;
            }
        }
        [HttpGet("/GetDirMembershipDirectors/{ShortForm}/{NextYear}")]
        public IEnumerable<DirMain> GetDirMembershipDirectors(int ShortForm, int NextYear)
        {
            try
            {
                return _context.Database
                    .SqlQuery<DirMain>($"[uspRPT_DirectoryMemberShipDirectors] {ShortForm},{NextYear}")
                    .ToList();
            }
            catch (Exception ex)
            {
                Log.Fatal(GetType() + " - " + ex.Message + " " + ex.InnerException);
                var _new = new List<DirMain>();
                _new = null;
                return _new;
            }
        }
        [HttpGet("/GetDirProgramDirectors/{ShortForm}/{NextYear}")]
        public IEnumerable<DirMain> GetDirProgramDirectors(int ShortForm, int NextYear)
        {
            try
            {
                return _context.Database
                    .SqlQuery<DirMain>($"[uspRPT_DirectoryProgramDirectors] {ShortForm},{NextYear}")
                    .ToList();
            }
            catch (Exception ex)
            {
                Log.Fatal(GetType() + " - " + ex.Message + " " + ex.InnerException);
                var _new = new List<DirMain>();
                _new = null;
                return _new;
            }
        }
        [HttpGet("/GetDirOtherChairmen/{ShortForm}/{NextYear}")]
        public IEnumerable<DirMain> GetDirOtherChairmen(int ShortForm, int NextYear)
        {
            try
            {
                return _context.Database
                    .SqlQuery<DirMain>($"[uspRPT_DirectoryOtherChairmen] {ShortForm},{NextYear}")
                    .ToList();
            }
            catch (Exception ex)
            {
                Log.Fatal(GetType() + " - " + ex.Message + " " + ex.InnerException);
                var _new = new List<DirMain>();
                _new = null;
                return _new;
            }
        }
        [HttpGet("/GetDirTechnologyChairmen/{ShortForm}/{NextYear}")]
        public IEnumerable<DirMain> GetDirTechnologyChairmen(int ShortForm, int NextYear)
        {
            try
            {
                return _context.Database
                    .SqlQuery<DirMain>($"[uspRPT_DirectoryTechnologyChairmen] {ShortForm},{NextYear}")
                    .ToList();
            }
            catch (Exception ex)
            {
                Log.Fatal(GetType() + " - " + ex.Message + " " + ex.InnerException);
                var _new = new List<DirMain>();
                _new = null;
                return _new;
            }
        }
        [HttpGet("/GetDirAandFChairmen/{ShortForm}/{NextYear}")]
        public IEnumerable<DirMain> GetDirAandFChairmen(int ShortForm, int NextYear)
        {
            try
            {
                return _context.Database
                    .SqlQuery<DirMain>($"[uspRPT_DirectoryAandFChairmen] {ShortForm},{NextYear}")
                    .ToList();
            }
            catch (Exception ex)
            {
                Log.Fatal(GetType() + " - " + ex.Message + " " + ex.InnerException);
                var _new = new List<DirMain>();
                _new = null;
                return _new;
            }
        }
        [HttpGet("/GetDirPSDs/{ShortForm}/{NextYear}")]
        public IEnumerable<DirMain> GetDirPSDs(int ShortForm, int NextYear)
        {
            try
            {
                return _context.Database
                    .SqlQuery<DirMain>($"[uspRPT_DirectoryPSD] {ShortForm},{NextYear}")
                    .ToList();
            }
            catch (Exception ex)
            {
                Log.Fatal(GetType() + " - " + ex.Message + " " + ex.InnerException);
                var _new = new List<DirMain>();
                _new = null;
                return _new;
            }
        }
        [HttpGet("/GetDirFFLs/{ShortForm}/{NextYear}")]
        public IEnumerable<DirMain> GetDirFFLs(int ShortForm, int NextYear)
        {
            try
            {
                return _context.Database
                    .SqlQuery<DirMain>($"[uspRPT_DirectoryFFL] {ShortForm},{NextYear}")
                    .ToList();
            }
            catch (Exception ex)
            {
                Log.Fatal(GetType() + " - " + ex.Message + " " + ex.InnerException);
                var _new = new List<DirMain>();
                _new = null;
                return _new;
            }
        }
        [HttpGet("/GetDirNSDandDIG/{ShortForm}/{NextYear}")]
        public IEnumerable<DirMain> GetDirNSDandDIG(int ShortForm, int NextYear)
        {
            try
            {
                return _context.Database
                    .SqlQuery<DirMain>($"[uspRPT_DirectoryNSDandDIG] {ShortForm},{NextYear}")
                    .ToList();
            }
            catch (Exception ex)
            {
                Log.Fatal(GetType() + " - " + ex.Message + " " + ex.InnerException);
                var _new = new List<DirMain>();
                _new = null;
                return _new;
            }
        }
        [HttpGet("/GetDirIGA/{ShortForm}/{NextYear}")]
        public IEnumerable<DirMain> GetDirIGA(int ShortForm, int NextYear)
        {
            try
            {
                return _context.Database
                    .SqlQuery<DirMain>($"[uspRPT_DirectoryIGA] {ShortForm},{NextYear}")
                    .ToList();
            }
            catch (Exception ex)
            {
                Log.Fatal(GetType() + " - " + ex.Message + " " + ex.InnerException);
                var _new = new List<DirMain>();
                _new = null;
                return _new;
            }
        }
        [HttpGet("/GetDirIRep/{ShortForm}/{NextYear}")]
        public IEnumerable<DirMain> GetDirIRep(int ShortForm, int NextYear)
        {
            try
            {
                return _context.Database
                    .SqlQuery<DirMain>($"[uspRPT_DirectoryIRep] {ShortForm},{NextYear}")
                    .ToList();
            }
            catch (Exception ex)
            {
                Log.Fatal(GetType() + " - " + ex.Message + " " + ex.InnerException);
                var _new = new List<DirMain>();
                _new = null;
                return _new;
            }
        }
        [HttpGet("/GetDirCouncilSum/{ShortForm}/{NextYear}")]
        public IEnumerable<DirMain> GetDirCouncilSum(int ShortForm, int NextYear)
        {
            try
            {
                return _context.Database
                    .SqlQuery<DirMain>($"[uspRPT_DirectoryCouncilSummary] {ShortForm},{NextYear}")
                    .ToList();
            }
            catch (Exception ex)
            {
                Log.Fatal(GetType() + " - " + ex.Message + " " + ex.InnerException);
                var _new = new List<DirMain>();
                _new = null;
                return _new;
            }
        }
        [HttpGet("/GetDir4thDegreeOfficers/{ShortForm}/{NextYear}")]
        public IEnumerable<DirMain> GetDir4thDegreeOfficers(int ShortForm, int NextYear)
        {
            try
            {
                return _context.Database
                    .SqlQuery<DirMain>($"[uspRPT_Directory4thDegreeOfficers] {ShortForm},{NextYear}")
                    .ToList();
            }
            catch (Exception ex)
            {
                Log.Fatal(GetType() + " - " + ex.Message + " " + ex.InnerException);
                var _new = new List<DirMain>();
                _new = null;
                return _new;
            }
        }
        [HttpGet("/GetDirCouncils/{ShortForm}/{NextYear}")]
        public IEnumerable<DirMain> GetDirCouncils(int ShortForm, int NextYear)
        {
            try
            {
                return _context.Database
                    .SqlQuery<DirMain>($"[uspRPT_DirectoryCouncils] {ShortForm},{NextYear}")
                    .ToList();
            }
            catch (Exception ex)
            {
                Log.Fatal(GetType() + " - " + ex.Message + " " + ex.InnerException);
                var _new = new List<DirMain>();
                _new = null;
                return _new;
            }
        }
        [HttpGet("/GetDirFVSMFMFD/{ShortForm}/{NextYear}")]
        public IEnumerable<DirMain> GetDirFVSMFMFD(int ShortForm, int NextYear)
        {
            try
            {
                return _context.Database
                    .SqlQuery<DirMain>($"[uspRPT_DirectoryFVSMFMFD] {ShortForm},{NextYear}")
                    .ToList();
            }
            catch (Exception ex)
            {
                Log.Fatal(GetType() + " - " + ex.Message + " " + ex.InnerException);
                var _new = new List<DirMain>();
                _new = null;
                return _new;
            }
        }
        [HttpGet("/GetDirAssySum/{ShortForm}/{NextYear}")]
        public IEnumerable<DirMain> GetDirAssySum(int ShortForm, int NextYear)
        {
            try
            {
                return _context.Database
                    .SqlQuery<DirMain>($"[uspRPT_DirectoryAssemblySummary] {ShortForm},{NextYear}")
                    .ToList();
            }
            catch (Exception ex)
            {
                Log.Fatal(GetType() + " - " + ex.Message + " " + ex.InnerException);
                var _new = new List<DirMain>();
                _new = null;
                return _new;
            }
        }
        [HttpGet("/GetDirAssemblies/{ShortForm}/{NextYear}")]
        public IEnumerable<DirMain> GetDirAssemblies(int ShortForm, int NextYear)
        {
            try
            {
                return _context.Database
                    .SqlQuery<DirMain>($"[uspRPT_DirectoryAssemblies] {ShortForm},{NextYear}")
                    .ToList();
            }
            catch (Exception ex)
            {
                Log.Fatal(GetType() + " - " + ex.Message + " " + ex.InnerException);
                var _new = new List<DirMain>();
                _new = null;
                return _new;
            }
        }
        [HttpGet("/GetRollcallDistricts/{Day}")]
        public IEnumerable<RollCallSheets> GetRollCallSheets(string Day)
        {
            try
            {
                return _context.Database
                    .SqlQuery<RollCallSheets>($"[uspRPT_GetRollCallDistricts] {Day}")
                    .ToList();
            }
            catch (Exception ex)
            {

                Log.Fatal(GetType() + " - " + ex.Message + " " + ex.InnerException);
                var _new = new List<RollCallSheets>();
                _new = null;
                return _new;
            }
        }
        [HttpGet("/GetRollcallDDs/{Day}")]
        public IEnumerable<RollCallSheets> GetRollCallDDs(string Day)
        {
            try
            {
                return _context.Database
                    .SqlQuery<RollCallSheets>($"[uspRPT_GetRollCallDDs] {Day}")
                    .ToList();
            }
            catch (Exception ex)
            {

                Log.Fatal(GetType() + " - " + ex.Message + " " + ex.InnerException);
                var _new = new List<RollCallSheets>();
                _new = null;
                return _new;
            }
        }
        [HttpGet("/GetRollcallDelegates/{Day}")]
        public IEnumerable<RollCallSheets> GetRollCallDelegates(string Day)
        {
            try
            {
                return _context.Database
                    .SqlQuery<RollCallSheets>($"[uspRPT_GetRollCallDelegates] {Day}")
                    .ToList();
            }
            catch (Exception ex)
            {

                Log.Fatal(GetType() + " - " + ex.Message + " " + ex.InnerException);
                var _new = new List<RollCallSheets>();
                _new = null;
                return _new;
            }
        }
        [HttpGet("/GetRollcallOthers/{Day}")]
        public IEnumerable<RollCallSheets> GetRollCallOthers(string Day)
        {
            try
            {
                return _context.Database
                    .SqlQuery<RollCallSheets>($"[uspRPT_GetRollCallOthers] {Day}")
                    .ToList();
            }
            catch (Exception ex)
            {

                Log.Fatal(GetType() + " - " + ex.Message + " " + ex.InnerException);
                var _new = new List<RollCallSheets>();
                _new = null;
                return _new;
            }
        }
    }
}
