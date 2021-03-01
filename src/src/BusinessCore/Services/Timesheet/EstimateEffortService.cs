using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.eShopWeb.BusinessCore.Interfaces;
using Microsoft.eShopWeb.Infrastructure.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.eShopWeb.BusinessCore.Services
{
    public class EstimateEffortService : BaseService<EstimateEffort>, IEstimateEffortService
    {
        public EstimateEffortService(CatalogContext dbContext)
            : base(dbContext)
        {

        }

        public async Task DeleteByProject(string projectGid)
        {
            string sql = "truncate table [EstimateEffort] where projectGid = projectGid";
            await SqlExecuteNonQuery(sql);
        }


        public async Task<int> Update(string projectGid, List<EstimateEffort> efforts)
        {
            // 删掉原来的，重新插入新版本的。
            await DeleteByProject(projectGid);

            var cnt = await AddManyAsync(efforts);

            return cnt;
        }
    }
}
