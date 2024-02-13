namespace docuverwalter_api.Models.Dto.DocumentDto
{
    public class DocumentShareLinkDto
    {
        public Guid ShareLinkId { get; set; }
        public Guid DocumentId { get; set; }
        public string GeneratedLink { get; set; } = string.Empty;
        public DateTime ExpiryDateTime { get; set; }
        public bool IsActive { get; set; }

    }
}
