using Blip.Api.Template.Models;
using Lime.Protocol;
using System.Threading;
using System.Threading.Tasks;

namespace Blip.Api.Template.Services
{
    public interface IContextManager
    {
        /// <summary>
        /// Saves the context attached to the user using IBucketExtension
        /// </summary>
        /// <param name="user"></param>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task SetUserContextAsync(Node user, UserContext context, CancellationToken cancellationToken);

        /// <summary>
        /// Gets the user's context from the IBucketExtension
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<UserContext> GetUserContextAsync(Node user, CancellationToken cancellationToken);

        /// <summary>
        /// Gets the bucket key related to the context
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        string GetBucketKey(Node user);
    }
}
