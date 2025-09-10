using BookStore.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using FV = FluentValidation;


namespace BookStoreApp.Client.Shared.Templates.FormBases;
public abstract class FormBase<TRequest, TResponse> : ComponentBase
    where TRequest : class, new()
    where TResponse : notnull, new()
{
    [Inject] protected IDialogService DialogService { get; set; } = default!;
    [Inject] protected ISnackbar SnackbarFormBase { get; set; } = default!;
    [Inject] protected FV.AbstractValidator<TRequest>? Validator { get; set; }

    [Parameter] public Action<TResponse> SuccessCallBack { get; set; } = default!;
    [Parameter] public Action<ErrorRequest> FailCallBack { get; set; } = default!;

    [Parameter] public string? SuccessMessage { get; set; } = null;
    [Parameter] public string? FailedMessage { get; set; } = null;
    [Parameter] public bool DisableSuccessDefaultMessage { get; set; } = false;
    [Parameter] public bool DisableFailDefaultMessage { get; set; } = false;
    [Parameter] public string? ButtonSubmitText { get; set; } = null;

    public TRequest Model
    {
        get => _model;
        set
        {
            _model = value;
            StateHasChanged();
        }
    }

    private TRequest _model = new();
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
            _response = await SendMessage.Invoke();
            await _response.Match(
               success: async succ =>
               {
                   await Reset();
                   await Success(succ);
               },
               failure: async fail =>
               {
                   await Fail(fail);
               });
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

    public required Func<Task<Result<TResponse>>> SendMessage;

    protected virtual async Task Fail(ErrorRequest bad)
    {
        ShowFailMessage(bad.Message);
        await Task.CompletedTask;
    }
    protected virtual async Task Success(TResponse response)
    {
        ShowSuccesMessage();
        await Task.CompletedTask;
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

    protected virtual async Task Cancel()
    {
        ResetCancelationToken();
        await Task.CompletedTask;
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

    public Func<object, string, IEnumerable<string>> ValidateValue => (mod, propertyName) =>
    {
        if (Validator is null)
        {
            return [];
        }
        var result = Validator.Validate(
            FV.ValidationContext<TRequest>
                .CreateWithOptions((TRequest)mod, x => x.IncludeProperties(propertyName)));
        return
            result.IsValid
            ? []
            : result.Errors.Select(e => e.ErrorMessage);
    };

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
