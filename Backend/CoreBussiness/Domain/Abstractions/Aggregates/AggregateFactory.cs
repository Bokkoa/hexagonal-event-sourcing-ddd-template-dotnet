using Domain.Modules.Todos.Aggregates;

namespace Domain.Abstractions.Aggregates;
public static class AggregateFactory
{
    public static T CreateAggregate<T>() where T: class
    {
        Console.WriteLine(typeof(T).Name);
        Console.WriteLine("@@@@@@@@@@");

        T? aggregate = typeof(T).Name switch
        {
            nameof(TodoAggregate) => new TodoAggregate() as T,
            _ => throw new InvalidOperationException($"No factory aggregate registered for type {typeof(T).Name}")
        };

        if(aggregate == null) throw new InvalidOperationException($"No factory aggregate type was given");

        return aggregate;

    }
}
