using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace KofCWSC.API.Models
{
    public class SPFourthDegreeOfficersViewModel
    {
        public List<SPGetChairmanInfoBlock>? FourthDegreeOfficers { get; set; }
    }
}
