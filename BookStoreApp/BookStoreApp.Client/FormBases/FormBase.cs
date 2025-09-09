using BookStore.Models;
using BookStoreApp.Client;
using BookStoreApp.Client.Shared;
using LanguageExt;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Net;

namespace BookStoreApp.Components.FormBases;
public abstract class FormBase<TService, TRequest, TResponse> : ComponentBase
    where TService : IService
    where TRequest : class, new()
    where TResponse : notnull, new()
{
    [Inject] protected IDialogService DialogService { get; set; } = default!;
    [Inject] protected ISnackbar SnackbarFormBase { get; set; } = default!;
    [Inject] protected TService Service { get; set; } = default!;

    [Parameter] public Action<Result<TResponse>> SuccessCallBack { get; set; } = default!;
    [Parameter] public Action<Result<TResponse>> FailCallBack { get; set; } = default!;


    // [Parameter] public string HttpClientName { get; set; } = default!;
    // [Parameter] public string Endpoint { get; set; } = default!;
    [Parameter] public string? SuccessMessage { get; set; } = null;
    [Parameter] public string? FailedMessage { get; set; } = null;
    [Parameter] public bool DisableSuccessDefaultMessage { get; set; } = false;
    [Parameter] public bool DisableFailDefaultMessage { get; set; } = false;
    [Parameter] public string? ButtonSubmitText { get; set; } = null;

    
    protected TRequest _model = new();
    protected Result<TResponse> _response = default!;

    protected CancellationTokenSource cancellationTokenSource = new();

    protected MudForm? form;

    protected async Task Submit()
    {
        await form!.Validate();

        if (!form.IsValid)
        {
            return;
        }

        var dlgRef = ShowDialog();

        try
        {
            _response = await SendMessage();

            //switch (_response.StatusCode)
            //{
            //    case HttpStatusCode.OK:
            //        await Reset();
            //        await Success();
            //        break;
            //    case HttpStatusCode.Unauthorized:
            //    case HttpStatusCode.Forbidden:
            //        SnackbarFormBase.Add(
            //            message: "User doesn't have permission.",
            //            severity: Severity.Error);
            //        break;
            //    case HttpStatusCode.InternalServerError:
            //        SnackbarFormBase.Add(
            //            message: "An unexpected error has occurred.",
            //            severity: Severity.Error);
            //        break;

            //    case HttpStatusCode.BadRequest:
            //    default:
            //        await Fail();
            //        break;
            //}
        }
        catch (TaskCanceledException ex) when (ex.CancellationToken.IsCancellationRequested)
        {
            SnackbarFormBase.Add("Request canceled by user!", Severity.Warning);
        }
        catch (TaskCanceledException)
        {
            SnackbarFormBase.Add("Request timed out", Severity.Warning);
        }
        catch (Exception ex)
        {
            SnackbarFormBase.Add($"Unexpected error:<BR/><BR>{ex.Message}", Severity.Error);
        }
        finally
        {
            ResetCancelationToken();
            (await dlgRef)?.Close();
        }
    }

    protected abstract Task<Result<TResponse>> SendMessage();

    protected virtual async Task Fail()
    {
        //var bad = await response.Content.ReadFromJsonAsync<BadRequestResponse>();
        //ShowFailMessage(bad);
        await Task.CompletedTask;
    }
    protected virtual Task Success()
    {
        ShowSuccesMessage();
        return Task.CompletedTask;
    }

    protected void ShowFailMessage(string? msg = null)
    {
        if (FailedMessage is not null)
        {
            SnackbarFormBase.Add(
                message: FailedMessage,
                severity: Severity.Error);
        }
        else if (DisableFailDefaultMessage == false)
        {
            SnackbarFormBase.Add(
                message: msg ?? "msg",
                severity: Severity.Error);
        }
    }

    protected void ShowSuccesMessage()
    {
        if (SuccessMessage is not null)
        {
            SnackbarFormBase.Add(
                message: SuccessMessage,
                severity: Severity.Success);
        }
        else if (DisableFailDefaultMessage == false)
        {
            SnackbarFormBase.Add(
                message: "Completed successfully",
                severity: Severity.Success);
        }
    }

    private async Task<IDialogReference?> ShowDialog()
    {
        try
        {
            await Task.Delay(1_000, cancellationTokenSource.Token);
            return await DialogService.ShowAsync<CancelDialog>("",
                new DialogParameters
                {
                    {"cancellationTokenSource", cancellationTokenSource }
                },
                new DialogOptions
                {
                    CloseOnEscapeKey = false,
                    // DisableBackdropClick = true,
                    CloseButton = false,
                    NoHeader = true,

                });
        }
        catch (TaskCanceledException)
        {
            return null;
        }
    }

    protected virtual Task Cancel()
    {
        ResetCancelationToken();
        return Task.CompletedTask;
    }
    protected virtual async Task Reset()
    {
        if (form is not null)
        {
            await form.ResetAsync();
        }
        await Task.CompletedTask;
    }
    private void ResetCancelationToken()
    {
        cancellationTokenSource.Cancel();
        cancellationTokenSource.Dispose();
        cancellationTokenSource = new CancellationTokenSource();
    }

    //protected override async Task OnInitializedAsync()
    //{
    //    await base.OnInitializedAsync();

    //    HotKeysContext = HotKeysHandle.Add()
    //        .Add(ModKeys.Ctrl, Keys.Enter, Submit, ButtonSubmitText, exclude: Exclude.InputNonText | Exclude.TextArea | Exclude.InputNonText);

    //}
    //public void Dispose()
    //{
    //    HotKeysHandle.Remove();
    //}
}
