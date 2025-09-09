using System.Text.Json;

namespace BookStoreApp.Client;
public static class ObjectExtension
{
    public static T CloneJson<T>(this T source)
        where T : new() => source is null ? new T(): JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(source))!;

}
