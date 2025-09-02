using System.Text;
using System.Text.Json;

namespace BookStoreApp.Client;
public static class ObjectExtension
{
    private readonly static JsonSerializerOptions _deserializeSettings = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    public static T CloneJson<T>(this T source) => source is null
            ? default!
            : JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(source), _deserializeSettings)!;
}
