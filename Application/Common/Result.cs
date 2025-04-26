namespace Application.Common;

public class Result
{
    public bool IsSuccess { get; }
    public string Error { get; }

    public bool IsFailure => !IsSuccess;

    protected Result(bool isSuccess, string error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new Result(true, string.Empty);
    public static Result Failure(string error) => new Result(false, error);
}

public class Result<T> : Result
{
    protected Result(bool isSuccess, string error, T value) : base(isSuccess, error)
    {
        Value = value;
    }
    public T Value { get; }
    public static Result<T> Success(T value) => new Result<T>(true, string.Empty,value);
    public new static Result<T> Failure(string error) => new Result<T>(false, error,default!);
}