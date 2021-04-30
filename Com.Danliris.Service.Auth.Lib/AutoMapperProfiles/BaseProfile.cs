using AutoMapper;
using Com.Danliris.Service.Auth.Lib.Models;
using Com.Danliris.Service.Auth.Lib.Utilities.BaseClass;
using Com.Danliris.Service.Auth.Lib.ViewModels;
using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Auth.Lib.AutoMapperProfiles
{
    public class BaseProfile : Profile
    {
        public BaseProfile()
        {
            CreateMap<StandardEntity, BaseOldViewModel>()
                .Include<Role, RoleViewModel>()
                .Include<Account, AccountViewModel>()
                .ForPath(d => d._active, opt => opt.MapFrom(s => s.Active))
                .ForPath(d => d._createAgent, opt => opt.MapFrom(s => s.CreatedAgent))
                .ForPath(d => d._createdBy, opt => opt.MapFrom(s => s.CreatedBy))
                .ForPath(d => d._createdDate, opt => opt.MapFrom(s => s.CreatedUtc))
                .ForPath(d => d._deleted, opt => opt.MapFrom(s => s.IsDeleted))
                .ForPath(d => d._id, opt => opt.MapFrom(s => s.Id))
                .ForPath(d => d._updateAgent, opt => opt.MapFrom(s => s.LastModifiedAgent))
                .ForPath(d => d._updatedBy, opt => opt.MapFrom(s => s.LastModifiedBy))
                .ForPath(d => d._updatedDate, opt => opt.MapFrom(s => s.LastModifiedUtc))
                .ReverseMap();
        }
    }
}
