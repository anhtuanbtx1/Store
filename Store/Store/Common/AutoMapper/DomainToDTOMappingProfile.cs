using AutoMapper;
using Store.Domain.Entity;
using Store.Models.Request;
using Store.Models.Respone;
using Store.Models.Response;

namespace Store.Common.AutoMapper
{
    public class DomainToDTOMappingProfile : Profile
    {
        public DomainToDTOMappingProfile()
        {
            CreateMap<Category, CategoryResponseModel>();
            CreateMap<CategoryResponseModel, Category>();

            CreateMap<News, NewsResponseModel>();
            CreateMap<NewResponseModel, News>();
            CreateMap<News, NewResponseModel>();

            CreateMap<ProductResponseModel, Product>();
            CreateMap<Product, ProductResponseModel>();
            CreateMap<ProductRequestModel, Product>();

            CreateMap<Banner, BannerResponseModel>();
        }
    }
}
