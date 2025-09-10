namespace BookStoreApp.Client.Shared.Templates.FormBases;

public interface IService
{
    void CancelCall();
}

public abstract class Service : IService
{
    protected CancellationTokenSource CancTokenSource { get; set; } = new CancellationTokenSource();
    public void CancelCall()
    {
        CancTokenSource.Cancel();
        CancTokenSource.Dispose();
        CancTokenSource = new CancellationTokenSource();
    }
}
