using System.ComponentModel.DataAnnotations;

namespace docuverwalter_api.Models
{
    public class Document
    {
        [Key]
        public Guid DocumentId { get; set; }

        [Required]
        [StringLength(255)]
        public string DocumentName { get; set; } = string.Empty;

        public string? DocumentType { get; set; }

        public string FilePath { get; set; } = string.Empty;

        public long FileSize { get; set; }

        public DateTime UploadDateTime { get; set; }
        public DateTime? LastEditedTime { get; set; }

        public int NumberOfDownloads { get; set; }
        public string? ApplicationUserId { get; set; }

        public ApplicationUser? ApplicationUser { get; set;}

        public int DocumentShareLinkId { get; set; }
        public List<DocumentShareLink>? DocumentShareLinks { get; set; }
    }
}
