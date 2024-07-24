using Microsoft.EntityFrameworkCore;

namespace KofCWSC.API.Models
{
    [Keyless]
    public class SPGetBulletins
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public string? Bulletin {  get; set; }
    }
}
