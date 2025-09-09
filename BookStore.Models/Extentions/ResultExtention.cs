using System.Net.Http.Json;

namespace BookStore.Models.Extentions;
public static class ResultExtention
{
    public static async Task<Result<TValue>> ToResult<TValue>(this HttpResponseMessage value)
    {
        if (value.IsSuccessStatusCode)
        {
            var succ = await value.Content.ReadFromJsonAsync<TValue>();
            return new Result<TValue>(succ!);
        }
        var bad = await value.Content.ReadFromJsonAsync<ErrorRequest>();
        return new Result<TValue>(bad!);
    }

    public static async Task<Result<TValue, ErrorRequest>> ToResult<TValue, TError>(this HttpResponseMessage value)
    {
        if (value.IsSuccessStatusCode)
        {
            var succ = await value.Content.ReadFromJsonAsync<TValue>();
            return new Result<TValue, ErrorRequest>(succ!);
        }

        var bad = await value.Content.ReadFromJsonAsync<ErrorRequest>();
        return new Result<TValue, ErrorRequest>(bad!);

    }
}
