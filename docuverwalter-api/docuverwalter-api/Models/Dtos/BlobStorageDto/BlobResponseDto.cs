namespace docuverwalter_api.Models.Dtos.BlobStorageDto
{
    public class BlobResponseDto
    {
        public BlobResponseDto()
        {
            Blob = new BlobRequestDto();
        }
        public string? Status {  get; set; }
        public bool Error {  get; set; }
        public BlobRequestDto Blob {  get; set; }
    }
}
