using Microsoft.AspNetCore.Components;

namespace BookStoreApp.Client.Shared.Templates.FormBases;
public abstract class FormTemplateServiceBase<TService, TRequest, TResponse> : FormTemplateBase<TRequest, TResponse>
    where TService : IService
    where TRequest : class, new()
    where TResponse : notnull, new()
{
    [Inject] public TService Service { get; set; } = default!;

    protected override Task Reset()
    {
        Service.CancelCall();
        return base.Reset();
    }

    protected override void ResetCancelationToken()
    {
        Service.CancelCall();
        base.ResetCancelationToken();
    }
}
