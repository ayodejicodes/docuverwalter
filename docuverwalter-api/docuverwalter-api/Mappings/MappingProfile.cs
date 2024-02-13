using docuverwalter_api.Models.Dtos.DocumentDto;
using docuverwalter_api.Models;
using AutoMapper;
using docuverwalter_api.Models.Dtos;

namespace docuverwalter_api.Mappings
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {

            CreateMap<DocumentCreateDto, Document>()
                .ForMember(dest => dest.DocumentName, opt => opt.Ignore()) 
                .ForMember(dest => dest.FilePath, opt => opt.Ignore()) 
                .ForMember(dest => dest.DocumentType, opt => opt.Ignore())
                .ForMember(dest => dest.FileSize, opt => opt.Ignore())
                .ForMember(dest => dest.ApplicationUser, opt => opt.Ignore());



            CreateMap<DocumentUpdateDto, Document>()
                .ForMember(dest => dest.DocumentId, opt => opt.Ignore());

            //CreateMap<Document, DocumentDto>();
            //CreateMap<DocumentShareLink, DocumentShareLinkDto>();
        }
    }
}
