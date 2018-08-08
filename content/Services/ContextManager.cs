using Blip.Api.Template.Models;
using Lime.Protocol;
using System;
using System.Threading;
using System.Threading.Tasks;
using Take.Blip.Client.Extensions.Bucket;

namespace Blip.Api.Template.Services
{
    public class ContextManager : IContextManager
    {
        private readonly IBucketExtension _bucket;

        public ContextManager(IBucketExtension bucket)
        {
            _bucket = bucket;
        }
        public string GetBucketKey(Node user) => $"{user.Name}_UserContext";


        public async Task<UserContext> GetUserContextAsync(Node user, CancellationToken cancellationToken)
        {
            try
            {

                var context = await _bucket.GetAsync<UserContext>(GetBucketKey(user), cancellationToken);

                if (context == null)
                {
                    context = new UserContext();
                    await _bucket.SetAsync(GetBucketKey(user), context, cancellationToken: cancellationToken);
                }

                return context;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Task SetUserContextAsync(Node user, UserContext context, CancellationToken cancellationToken)
        {
            return _bucket.SetAsync(GetBucketKey(user), context, cancellationToken: cancellationToken);
        }
    }
}
