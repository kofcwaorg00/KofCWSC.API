using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KofCWSC.API.Models
{
    [Table("tblSYS_trxEvents")]
    public class Event
    {
        // Primary key: Id (int, identity)
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int EventId { get; set; }

        // Title (nvarchar(50), not null)
        [Required]
        [StringLength(50)]
        [Column("Title")]
        public string Title { get; set; } = default!;

        // Description (nvarchar(max), not null)
        [Required]
        [Column("Description")]
        public string Details { get; set; } = default!;

        // Begin (datetime, not null)
        [Required]
        [Column("Begin")]
        public DateTime StartDate { get; set; }

        // End (datetime, not null)
        [Required]
        [Column("End")]
        public DateTime EndDate { get; set; }

        // isAllDay (bit, null)
        [Column("isAllDay")]
        public bool? IsAllDay { get; set; }

        // AttachURL (nvarchar(250), null)
        [StringLength(250)]
        [Column("AttachURL")]
        public string? AttachmentUrl { get; set; }

        // AddedBy (nvarchar(50), null)
        [StringLength(50)]
        [Column("AddedBy")]
        public string? CreatedBy { get; set; }

        // DateAdded (datetime, null)
        [Column("DateAdded")]
        public DateTime? DateCreated { get; set; }
    }
}