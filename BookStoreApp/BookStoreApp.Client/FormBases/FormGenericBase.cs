using BookStoreApp.Client;
using Microsoft.AspNetCore.Components;

namespace BookStoreApp.Components.FormBases;
public abstract class FormTemplateBase<TService, TRequest, TResponse> : FormBase<TService, TRequest, TResponse>
    where TService : IService
    where TRequest : class, new()
    where TResponse : notnull, new()
{
//     [Inject] protected AbstractValidator<TRequest> Validator { get; set; } = default!;
    [Parameter] public TRequest DefaultModel { get; set; } = new();
    [Parameter] public RenderFragment HeaderTemplate { get; set; } = default!;
    [Parameter] public RenderFragment<TRequest> FormTemplate { get; set; } = default!;
    [Parameter] public RenderFragment ButtonsTemplate { get; set; } = default!;


    protected override async Task Fail()
    {
        await Task.Delay(100);

        //var bad = await response.Content.ReadFromJsonAsync<BadRequestResponse>()!;

        //ShowFailMessage(bad);

        //FailCallBack?.Invoke(bad!);
    }

    protected override async Task Success()
    {
        await Task.Delay(100);
        //var resp = await response.Content.ReadFromJsonAsync<TResponse>() ?? new TResponse();

        //ShowSuccesMessage();

        //SuccessCallBack?.Invoke(resp);
    }

    protected override async Task Reset()
    {
        await base.Reset();
        _model = DefaultModel.CloneJson();
        this.StateHasChanged();
    }
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        _model = DefaultModel.CloneJson();
    }

    //public Func<object, string, IEnumerable<string>> ValidateValue => (mod, propertyName) =>
    //{
    //    var result = Validator.Validate(ValidationContext<TRequest>.CreateWithOptions((TRequest)mod, x => x.IncludeProperties(propertyName)));
    //    return
    //        result.IsValid ?
    //        (IEnumerable<string>)Array.Empty<string>() :
    //        result.Errors.Select(e => e.ErrorMessage);
    //};
}
