
using KSOAdmin.Core.EFDbContext;
using KSOAdmin.IRepository.System;
using KSOAdmin.Models.DomainModels.System;

namespace KSOAdmin.Repository.System
{
    public class Sys_UserRepository:BasicRepository<Sys_User>,ISys_UserRepository, IDependency
    {
        public Sys_UserRepository(KSOContext DbContext) : base(DbContext)
        {
        }
    }
}
