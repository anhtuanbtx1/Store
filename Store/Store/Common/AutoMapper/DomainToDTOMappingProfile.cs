using AutoMapper;
using Store.Domain.Entity;
using Store.Models.Respone;

namespace Store.Common.AutoMapper
{
    public class DomainToDTOMappingProfile : Profile
    {
        public DomainToDTOMappingProfile()
        {
            CreateMap<Category, CategoryResponeModel>();
            CreateMap<CategoryResponeModel, Category>();
        }
    }
}
