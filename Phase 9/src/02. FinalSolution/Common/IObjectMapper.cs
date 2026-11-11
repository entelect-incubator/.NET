using System.Collections;
using System.Reflection;

public interface IObjectMapper
{
    TDestination Map<TDestination>(object source);
}

public sealed class ObjectMapper : IObjectMapper
{
    public TDestination Map<TDestination>(object source)
        => (TDestination)MapValue(source, typeof(TDestination));

    private static object MapValue(object source, Type destinationType)
    {
        if (source is null)
        {
            return null;
        }

        if (destinationType != typeof(string) &&
            destinationType.IsGenericType &&
            destinationType.GetGenericTypeDefinition() == typeof(List<>))
        {
            var itemType = destinationType.GetGenericArguments()[0];
            var result = (IList)Activator.CreateInstance(destinationType);
            foreach (var item in (IEnumerable)source)
            {
                result.Add(MapValue(item, itemType));
            }

            return result;
        }

        if (destinationType.IsAssignableFrom(source.GetType()))
        {
            return source;
        }

        var destination = Activator.CreateInstance(destinationType);
        foreach (var destinationProperty in destinationType.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                     .Where(property => property.CanWrite))
        {
            var sourceProperty = source.GetType().GetProperty(destinationProperty.Name);
            var sourceValue = sourceProperty?.GetValue(source);
            if (sourceValue is null)
            {
                continue;
            }

            destinationProperty.SetValue(
                destination,
                MapValue(sourceValue, destinationProperty.PropertyType));
        }

        return destination;
    }
}
