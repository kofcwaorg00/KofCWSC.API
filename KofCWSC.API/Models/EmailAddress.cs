using Microsoft.EntityFrameworkCore;

namespace KofCWSC.API.Models
{
    [Keyless]
    public class EmailAddress
    {
        public string Email { get; set; }
    }
}
