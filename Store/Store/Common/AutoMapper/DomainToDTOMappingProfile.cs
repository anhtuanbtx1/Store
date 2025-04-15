using AutoMapper;
using Store.Domain.Entity;
using Store.Models.Respone;

namespace Store.Common.AutoMapper
{
    public class DomainToDTOMappingProfile : Profile
    {
        public DomainToDTOMappingProfile()
        {
            CreateMap<Category, CategoryResponseModel>();
            CreateMap<CategoryResponseModel, Category>();

            CreateMap<News, NewsResponseModel>();
            CreateMap<NewReponseModel, News>();
            CreateMap<News, NewReponseModel>();

            CreateMap<ProductResponseModel, Product>();
            CreateMap<Product, ProductResponseModel>();

        }
    }
}
