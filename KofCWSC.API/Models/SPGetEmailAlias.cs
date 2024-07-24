using Microsoft.EntityFrameworkCore;

namespace KofCWSC.API.Models
{
    [Keyless]
    public class SPGetEmailAlias
    {
        public string? Office { get; set; }

        public string? Alias { get; set; }
    }
}
