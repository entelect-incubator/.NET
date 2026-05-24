namespace Api.Helpers;

using System.Collections.Concurrent;
using System.Reflection;
using Dispatch;

public static class DispatcherExtensions
{
    private static readonly ConcurrentDictionary<Type, MethodInfo> QueryMethodCache = new();
    private static readonly ConcurrentDictionary<Type, MethodInfo> SendMethodCache = new();

    // Extension that works without specifying generic parameters - infers from the query type
    public static dynamic Query(this Dispatcher dispatcher, dynamic query, CancellationToken ct = default)
    {
        Type queryType = query.GetType();

        MethodInfo method = QueryMethodCache.GetOrAdd(queryType, qt =>
        {
            Type queryInterface = qt.GetInterfaces()
                .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQuery<>))
                ?? throw new InvalidOperationException($"{qt.Name} does not implement IQuery<TResult>");
            Type resultType = queryInterface.GetGenericArguments()[0];

            return typeof(Dispatcher)
                .GetMethod(nameof(Dispatcher.Query), BindingFlags.Public | BindingFlags.Instance)!
                .MakeGenericMethod(qt, resultType);
        });

        return method.Invoke(dispatcher, new object[] { query, ct })!;
    }

    // Extension that works without specifying generic parameters - infers from the command type
    public static dynamic Send(this Dispatcher dispatcher, dynamic command, CancellationToken ct = default)
    {
        Type commandType = command.GetType();

        MethodInfo method = SendMethodCache.GetOrAdd(commandType, commandTypeKey =>
        {
            Type commandInterface = commandTypeKey.GetInterfaces()
                .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommand<>))
                ?? throw new InvalidOperationException($"{commandTypeKey.Name} does not implement ICommand<TResult>");
            Type resultType = commandInterface.GetGenericArguments()[0];

            return typeof(Dispatcher)
                .GetMethod(nameof(Dispatcher.Send), BindingFlags.Public | BindingFlags.Instance)!
                .MakeGenericMethod(commandTypeKey, resultType);
        });

        return method.Invoke(dispatcher, new object[] { command, ct })!;
    }

    public static Task Publish<TNotification>(this Dispatcher dispatcher, TNotification notification, CancellationToken ct = default)
        where TNotification : INotification
    {
        return dispatcher.Publish(notification, ct);
    }
}
