using LanguageExt.Common;
using System.Net;

namespace BookStore.Models;

//public string? Message { get; set; }
public record BadRequest(List<string>? Errors) : Error
{
    public int StatusCode { get; set; } = 400;

    public override string Message => throw new NotImplementedException();

    public override bool IsExceptional => throw new NotImplementedException();

    public override bool IsExpected => throw new NotImplementedException();

    public override bool Is<E>()
    {
        return this is E;
    }

    public override ErrorException ToErrorException()
    {
        throw new NotImplementedException();
    }
}
