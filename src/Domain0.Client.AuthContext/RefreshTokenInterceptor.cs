using Castle.DynamicProxy;
using System;
using System.Threading.Tasks;

namespace Domain0.Api.Client
{
    internal class RefreshTokenInterceptor : AsyncInterceptorBase
    {
        private readonly AuthenticationContext authContext;
        private readonly IClientLockScope clientScope;

        public RefreshTokenInterceptor(
            AuthenticationContext domain0AuthenticationContext,
            IClientLockScope clientLockScope)
        {
            authContext = domain0AuthenticationContext;
            clientScope = clientLockScope;
        }

        protected override async Task InterceptAsync(IInvocation invocation, Func<IInvocation, Task> proceed)
        {
            await authContext.Refresh().ConfigureAwait(false);

            using (await clientScope.RequestSetupLock.ReaderLockAsync())
            {
                await proceed(invocation).ConfigureAwait(false);
            }
        }

        protected override async Task<TResult> InterceptAsync<TResult>(IInvocation invocation, Func<IInvocation, Task<TResult>> proceed)
        {
            await authContext.Refresh().ConfigureAwait(false);

            using (await clientScope.RequestSetupLock.ReaderLockAsync())
            {
                return await proceed(invocation).ConfigureAwait(false);
            }
        }
    }
}