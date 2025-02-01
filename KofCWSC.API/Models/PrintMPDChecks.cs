using System;

namespace KofCWSC.API.Models
{
    public class PrintMPDChecks
    {
        public string Payee { get; set; }
        public int CheckNumber { get; set; }
        public decimal CheckTotal { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PostalCode { get; set; }
        public int Council { get; set; }
        public int District { get; set; }
        public string? Title { get; set; }
        public DateOnly CheckDate { get; set; }
        public int miles { get; set; }
        public int SeatedDays { get; set; }
        public string? SigImageID { get; set; }
        public bool PrintCheckNumber { get; set; }

    }
}
