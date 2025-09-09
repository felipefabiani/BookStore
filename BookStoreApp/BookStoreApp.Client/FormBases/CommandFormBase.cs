using System.Net.Http.Json;

namespace BookStoreApp.Components.FormBases;
//public class CommandFormBase<TRequest, TResponse> : FormBase<TRequest, TResponse>
//    where TRequest : class, new()
//    where TResponse : notnull, new()
//{
//    protected override async Task<HttpResponseMessage> SendMessage()
//    {
//        var http = HttpClient;
//        return await http.PostAsJsonAsync(Endpoint, _model, cancellationTokenSource.Token);
//    }
//}

//public class PutFormBase<TRequest, TResponse> : FormBase<TRequest, TResponse>
//    where TRequest : class, new()
//    where TResponse : notnull, new()
//{
//    protected override async Task<HttpResponseMessage> SendMessage()
//    {
//        return await HttpClient.PutAsJsonAsync(Endpoint, _model, cancellationTokenSource.Token);
//    }
//}

//public class DeleteFormBase : FormBase
//{
//    protected override async Task<HttpResponseMessage> SendMessage()
//    {
//        return await HttpClient.DeleteAsync(Endpoint, cancellationTokenSource.Token);
//    }
//}