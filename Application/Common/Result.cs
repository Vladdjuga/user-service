namespace Application.Common;

public interface IResult
{
    bool IsSuccess { get; }
    string Error { get; }
    bool IsFailure { get; }
}
public class Result:IResult
{
    public bool IsSuccess { get; }
    public string Error { get; }

    public bool IsFailure => !IsSuccess;

    protected Result(bool isSuccess, string error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public static IResult Success() => new Result(true, string.Empty);
    public static IResult Failure(string error) => new Result(false, error);
}

public class Result<T> : Result,IResult
{
    protected Result(bool isSuccess, string error, T value) : base(isSuccess, error)
    {
        Value = value;
    }
    public T Value { get; }
    public static Result<T> Success(T value) => new Result<T>(true, string.Empty,value);
    public new static Result<T> Failure(string error) => new Result<T>(false, error,default!);
}