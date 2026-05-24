#pragma warning disable SA1516

namespace Api.Helpers;

using System.Collections.Concurrent;
using System.Reflection;
using Core;

public static class DispatcherExtensions
{
	private static readonly ConcurrentDictionary<Type, MethodInfo> QueryMethodCache = new();

	private static readonly ConcurrentDictionary<Type, MethodInfo> SendMethodCache = new();

	public static dynamic Query(this Dispatcher dispatcher, dynamic query, CancellationToken ct = default)
	{
		Type queryType = query.GetType();

		var method = QueryMethodCache.GetOrAdd(queryType, GetQueryMethod);

		return method.Invoke(dispatcher, new object[] { query, ct });
	}

	public static dynamic Send(this Dispatcher dispatcher, dynamic command, CancellationToken ct = default)
	{
		Type commandType = command.GetType();

		var method = SendMethodCache.GetOrAdd(commandType, GetSendMethod);

		return method.Invoke(dispatcher, new object[] { command, ct });
	}

	public static Task Publish<TNotification>(this Dispatcher dispatcher, TNotification notification, CancellationToken ct = default)
		where TNotification : INotification => dispatcher.Publish(notification, ct);

	private static MethodInfo GetQueryMethod(Type queryType)
	{
		var queryInterface = queryType.GetInterfaces()
			.FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQuery<>))
			?? throw new InvalidOperationException($"{queryType.Name} does not implement IQuery<TResult}}");

		var resultType = queryInterface.GetGenericArguments()[0];

		return typeof(Dispatcher)
			.GetMethod(nameof(Dispatcher.Query), BindingFlags.Public | BindingFlags.Instance)!
			.MakeGenericMethod(queryType, resultType);
	}

	private static MethodInfo GetSendMethod(Type commandType)
	{
		var commandInterface = commandType.GetInterfaces()
			.FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommand<>))
			?? throw new InvalidOperationException($"{commandType.Name} does not implement ICommand<TResult}}");

		var resultType = commandInterface.GetGenericArguments()[0];

		return typeof(Dispatcher)
			.GetMethod(nameof(Dispatcher.Send), BindingFlags.Public | BindingFlags.Instance)!
			.MakeGenericMethod(commandType, resultType);
	}
}

#pragma warning restore SA1516
