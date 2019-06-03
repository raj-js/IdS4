using IdentityServer4.EntityFramework.Entities;
using System.Threading.Tasks;

namespace IdS4.Application.Interface
{
    /// <summary>
    /// Identity Resource 服务接口
    /// </summary>
    public interface IIdentityResourceService
    {
        /// <summary>
        /// 新增 Identity Resource
        /// </summary>
        /// <param name="identityResource"></param>
        /// <returns></returns>
        Task CreateAsync(IdentityResource identityResource);

        /// <summary>
        /// 修改 Identity Resource
        /// </summary>
        /// <param name="identityResource"></param>
        /// <returns></returns>
        Task ModifyAsync(IdentityResource identityResource);

        /// <summary>
        /// 删除 Identity Resource
        /// </summary>
        /// <param name="identityResource"></param>
        /// <returns></returns>
        Task RemoveAsync(IdentityResource identityResource);

        /// <summary>
        /// 根据 id 查找 Identity Resource
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IdentityResource> FindAsync(int id);

        /// <summary>
        /// 查找 Identity Resource
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        // Task<Paged<IdentityResource>> SearchAsync(PageQuery<IdentityResource> query);
    }
}
