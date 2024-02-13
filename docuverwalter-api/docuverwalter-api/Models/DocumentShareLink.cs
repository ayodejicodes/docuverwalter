using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace docuverwalter_api.Models
{
    public class DocumentShareLink
    {
        [Key]
        public Guid DocumentShareLinkId { get; set; }

        public string ShareLink { get; set; } = string.Empty;

        public DateTime ValidUntil { get; set; }

        public Guid DocumentId { get; set; }
        public Document Document { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
    }
}
