using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KSOAdmin.Core.EFDbContext;
using KSOAdmin.IRepository;
using KSOAdmin.IRepository.System;
using KSOAdmin.IServices.System;
using KSOAdmin.Models.DomainModels.System;

namespace KSOAdmin.Services.System
{
    public class Sys_ProvinceServices : BasicServices<Sys_Province>, ISys_ProvinceServices, IDependency
    {
        private readonly ISys_ProvinceRepository _pository;
        private readonly IBasicRepository<Sys_Province> _BaseRepository;
        public Sys_ProvinceServices(ISys_ProvinceRepository pository, IBasicRepository<Sys_Province> BaseRepository)
        {
            this._pository = pository;
            this.repository = BaseRepository;//父类
            this._BaseRepository = BaseRepository;
        }
        /// <summary>
        /// 配置化
        /// </summary>
        /// <typeparam name="Detail"></typeparam>
        /// <param name="queryeable"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        protected override object GetDetailSummary<Detail>(IQueryable<Detail> queryeable)
        {
            throw new NotImplementedException();
        }

        public Task<List<Sys_Province>> RegionList()
        {
            throw new NotImplementedException();
        }

       
    }
}
