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

            CreateMap<News, NewsResponseModel>()
            .ForMember(dest => dest.newsId, opts => opts.MapFrom(src => src.news_id))
            .ForMember(dest => dest.newsThumbnail, opts => opts.MapFrom(src => src.news_thumbnail))
            .ForMember(dest => dest.newsShortContent, opts => opts.MapFrom(src => src.news_short_content))
            .ForMember(dest => dest.newsDetailContent, opts => opts.MapFrom(src => src.news_detail_content))
            ;


            CreateMap<NewsResponseModel, News>();
        }
    }
}
