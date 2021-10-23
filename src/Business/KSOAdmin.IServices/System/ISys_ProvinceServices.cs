using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KSOAdmin.Models.DomainModels.System;

namespace KSOAdmin.IServices.System
{
    public interface ISys_ProvinceServices : IBasicServices<Sys_Province>
    {
        Task<List<Sys_Province>> RegionList();
    }
}
