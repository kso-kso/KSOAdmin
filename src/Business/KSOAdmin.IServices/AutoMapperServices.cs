using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;

using KSOAdmin.Models.DomainModels.System;
using KSOAdmin.Models.ViewModel;

namespace KSOAdmin.IServices
{
    public class AutoMapperServices :Profile
    {
        public AutoMapperServices()
        {
           CreateMap<Sys_User, View_Sys_User>().ReverseMap();
          //
          // CreateMap<CSComment, CSCommentViewModel>()
          //     .ForMember(d => d.LiveWorksId, opt => opt.MapFrom(s => s.CSWorksId))
          //     .ReverseMap();

        }
    }
}
