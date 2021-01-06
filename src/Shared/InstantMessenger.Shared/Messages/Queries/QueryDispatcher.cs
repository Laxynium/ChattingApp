using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace InstantMessenger.Shared.Messages.Queries
{
    internal sealed class QueryDispatcher : IQueryDispatcher
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public QueryDispatcher(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }
        public async Task<TResult> QueryAsync<TResult>(IQuery<TResult> query)
        {
            using var scope = _scopeFactory.CreateScope();
            var type = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));
            dynamic handler = scope.ServiceProvider.GetRequiredService(type);
            var task = (Task<TResult>)handler.HandleAsync((dynamic) query);
            return await task;
        }
    }
}