using IdS4.Application.Models.Paging;
using IdS4.Application.Models.Resource;
using System.Threading.Tasks;

namespace IdS4.Application.Queries
{
    public interface IResourceQueries
    {
        /// <summary>
        /// 查询身份资源
        /// </summary>
        /// <param name="query">分页查询参数</param>
        /// <returns></returns>
        Task<Paged<VmIdentityResource>> GetIdentityResources(PagingQuery query);

        /// <summary>
        /// 查询身份资源
        /// </summary>
        /// <param name="id">身份资源编号</param>
        /// <returns></returns>
        Task<VmIdentityResource> GetIdentityResource(int id);

        /// <summary>
        /// 查询Api资源
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<Paged<VmApiResource>> GetApiResources(PagingQuery query);

        /// <summary>
        /// 查询Api资源
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<VmApiResource> GetApiResource(int id);
    }
}
