using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace KofCWSC.API.Models
{
    public class TblValCouncilMPD
    {
        //public List<TblValCouncil> Councils { get; set; }
        public int CNumber { get; set; }
        public string? CName { get; set; }
        public int District { get; set; }
        public bool SeatedDelegateDay1D1 { get; set; }
        public bool SeatedDelegateDay1D2 { get; set; }
        public bool SeatedDelegateDay2D1 { get; set; }
        public bool SeatedDelegateDay2D2 { get; set; }
        public bool SeatedDelegateDay3D1 { get; set; }
        public bool SeatedDelegateDay3D2 { get; set; }
    }
}
