namespace BookStore.Api.Endpoints;

public interface IEndpoint
{
    void MapEndpopint();
}
public interface IEndpoint<TReq> : IEndpoint
{
    delegate Task LoginHandler(TReq request, CancellationToken cancellationToken);
}

public interface IEndpoint<TReq, TResp> : IEndpoint<TReq>
{
    new delegate Task<TResp> LoginHandler(TReq request, CancellationToken cancellationToken);
}