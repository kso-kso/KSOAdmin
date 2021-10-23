using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KSOAdmin.Core.EFDbContext;
using KSOAdmin.IRepository.System;
using KSOAdmin.Models.DomainModels.System;

namespace KSOAdmin.Repository.System
{
    /// <summary>
    /// 地址下拉框
    /// </summary>
    public class Sys_ProvinceRepository : BasicRepository<Sys_Province>, ISys_ProvinceRepository, IDependency
    {
        public Sys_ProvinceRepository(KSOContext DbContext) : base(DbContext)
        {

        }
    }
}
