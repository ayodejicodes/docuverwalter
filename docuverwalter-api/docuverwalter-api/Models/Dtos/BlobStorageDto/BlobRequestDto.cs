namespace docuverwalter_api.Models.Dtos.BlobStorageDto
{
    public class BlobRequestDto
    {
        public string? Uri { get; set; }
        public string? Name { get; set; }
        public string? ContentType { get; set; }
        public Stream? Content { get; set; }
    }
}
