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
    }
}
