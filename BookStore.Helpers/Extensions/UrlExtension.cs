using System.Collections;

namespace BookStore.Helpers.Extensions;

public static class UrlExtension
{
    public static string ToQueryString(this object request, string separator = ",")
    {
        // Get all properties on the object
        var properties = request.GetType().GetProperties()
            .Where(x => x.CanRead)
            .Where(x => x.GetValue(request, null) != null)
            .ToDictionary(x => x.Name, x =>
            {
                var obj = x.GetValue(request, null);
                return obj switch
                {
                    DateTime dt => dt.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffZ"), // JsonSerializer.Serialize(dt.ToUniversalTime()),
                    DateTimeOffset dt => dt.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                    _ => obj
                };
            });


        // Get names for all IEnumerable properties (excl. string)
        var propertyNames = properties
            .Where(x => x.Value is not string && x.Value is IEnumerable)
            .Select(x => x.Key)
            .ToList();

        // Concat all IEnumerable properties into a comma separated string
        foreach (var key in propertyNames)
        {
            var prop = properties[key];
            if (prop is null)
            {
                continue;
            }

            var valueType = prop.GetType();
            var valueElemType = valueType.IsGenericType
                                    ? valueType.GetGenericArguments()[0]
                                    : valueType.GetElementType();

            if (valueElemType?.IsPrimitive == true || 
                valueElemType == typeof(string))
            {
                var enumerable = properties[key] as IEnumerable;
                if (enumerable is not null)
                {
                    properties[key] = string.Join(separator, enumerable.Cast<object>());
                }
            }
        }

        // Concat all key/value pairs into a string separated by ampersand
        return string.Join("&", properties
            .Select(x => string.Concat(
                Uri.EscapeDataString(x.Key), "=",
                Uri.EscapeDataString(x.Value.ToString()))));
    }
}
