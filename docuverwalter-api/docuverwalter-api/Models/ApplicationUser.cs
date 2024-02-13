using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace docuverwalter_api.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;

        public DateTime DateCreated { get; set; }= DateTime.Now;
        public DateTime ProfileLastEditedTime { get; set; } = DateTime.Now;
        public DateTime? LastLogin { get; set; }

        public List<DocumentShareLink> DocumentShareLinks { get; set; } = new List<DocumentShareLink>();
    }
}
