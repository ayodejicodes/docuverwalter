using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace docuverwalter_api.Models
{
    public class DocumentShareLink
    {
        [Key]
        public Guid ShareLinkId { get; set; }

        [Required]
        public Guid DocumentId { get; set; }

        [Required]
        [StringLength(255)]
        public string GeneratedLink { get; set; } = string.Empty;

        [Required]
        public DateTime ExpiryDateTime { get; set; }

        [Required]
        public bool IsActive { get; set; }

        // Navigation property
        //public Document Document { get; set; }
    }
}
