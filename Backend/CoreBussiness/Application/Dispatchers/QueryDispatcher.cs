using Application.Abstractions;
using Application.Abstractions.Ports.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dispatchers;
public class QueryDispatcher<TModel> : IQueryDispatcher<TModel> where TModel : class
{
    private readonly Dictionary<Type, Func<BaseQuery, Task<List<TModel>>>> _handlers = new();

    public void RegisterHandler<TQuery>(Func<TQuery, Task<List<TModel>>> handler) where TQuery : BaseQuery
    {
        if (_handlers.ContainsKey(typeof(TQuery)))
        {
            throw new IndexOutOfRangeException("You Cannot Register the same query handler twice");
        }

        _handlers.Add(typeof(TQuery), x => handler((TQuery)x));
    }

    public async Task<List<TModel>> SendAsync(BaseQuery query)
    {
        if (_handlers.TryGetValue(query.GetType(), out Func<BaseQuery, Task<List<TModel>>> handler))
        {
            return await handler(query);
        }

        throw new ArgumentNullException(nameof(handler), "No query handler was registered!");
    }
}
