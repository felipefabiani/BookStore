
namespace BookStore.Models;

//public class ApiError
//{
//    public int StatusCode { get; set; } = 400;
//    public string Message { get; set; } = "Something went wrong.";
//    public List<string>? Details { get; set; }
//}

//public class FinResult<T>
//{
//    public Fin<T> Result { get; set; } = Fin<T>.Fail("Uninitialized");
//    public ApiError? ErrorDetails { get; set; }
//}
//public static class FinExtensions
//{
//    public static async Task<FinResult<T>> ToFinResultAsync<T>(this HttpResponseMessage response)
//    {
//        if (response.IsSuccessStatusCode)
//        {
//            var data = await response.Content.ReadFromJsonAsync<T>();
//            return new FinResult<T>
//            {
//                Result = Fin<T>.Succ(data!)
//            };
//        }

//        var error = await response.Content.ReadFromJsonAsync<ApiError>();
//        return new FinResult<T>
//        {
//            Result = Fin<T>.Fail(error?.Message ?? "Unknown error"),
//            ErrorDetails = error
//        };
//    }
//}
