using AutoMapper;
using SiteCDL.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteCDL.Application.Mappings
{
    public class MappingProfile : Profile
    {

        public MappingProfile()
        {
            CreateMap<Post, PostDto>()
                .ForMember(dest => dest.PostCategoria, opt => opt.MapFrom(src => src.PostCategoria));

            CreateMap<Banner, BannerDto>();
        }

    }
}
