using Microsoft.AspNetCore.Components;

namespace BookStoreApp.Client.Shared.Templates.FormBases;
public abstract class FormTemplateBase<TRequest, TResponse> : FormBase<TRequest, TResponse>
    where TRequest : class, new()
    where TResponse : notnull, new()
{
    [Parameter] public TRequest DefaultModel { get; set; } = new();
    [Parameter] public RenderFragment HeaderTemplate { get; set; } = default!;
    [Parameter] public RenderFragment<TRequest> FormTemplate { get; set; } = default!;
    [Parameter] public RenderFragment ButtonsTemplate { get; set; } = default!;
    [Parameter] public Action? ResetCallBack { get; set; } = null;

    protected override async Task Reset()
    {
        ResetCallBack?.Invoke();
        await base.Reset();
        Model = DefaultModel.CloneJson();
        StateHasChanged();
    }
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        Model = DefaultModel.CloneJson();
    }
}
