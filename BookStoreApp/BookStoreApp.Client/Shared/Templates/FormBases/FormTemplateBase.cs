using BookStore.Models;
using LanguageExt;
using LanguageExt.Common;
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

    [Parameter] public Action? ResetCallBack { get; set; }

    protected override async Task Fail(ErrorRequest err)
    {
        ShowFailMessage(err.Message);
        FailCallBack?.Invoke(err);
        await Task.CompletedTask;
    }

    protected override async Task Success(TResponse response)
    {
        await base.Success(response);
        SuccessCallBack?.Invoke(response);
    }

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
