namespace BookStoreApp.Client;


public class BadRequest
{
    public int StatusCode { get; set; }
    public string? Message { get; set; }
    public Errors? Errors { get; set; }
}

public class Errors
{
    public string[]? GeneralErrors { get; set; }
}

//public readonly struct Result<TValue, TError>
//{
//    private readonly TValue _value;
//    private readonly TError _error;

//    public Result(TValue value)
//    {
//        IsError = false;
//        _value = value;
//        _error = default;
//    }
//    public Result(TError error)
//    {
//        IsError = true;
//        _value = default;
//        _error = error;
//    }

//    public bool IsError { get;}
//    public bool IsSuccess => !IsError;

//    public static implicit operator Result<TValue, TError>(TValue value) => new(value);
//    public static implicit operator Result<TValue, TError>(TError error) => new(error);

//    public TResult Match<TResult>(
//        Func<TValue, TResult> success,
//        Func<TError, TResult> failure) =>
//            IsSuccess ? success(_value!) : failure(_error!);
//}