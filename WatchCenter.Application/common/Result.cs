
namespace WatchCenter.Application.common;

public class Result
{
    public bool Succeeded { get; set; }
    public string? Message { get; set; }
    public List<string>? Errors { get; set; }

    public static Result Success(string? message = null) => new Result { Succeeded = true, Message = message };
    public static Result Failure(string message, List<string>? errors = null)
        => new Result { Succeeded = false, Message = message, Errors = errors };

    public static Result Failure(string message, string error)
        => new Result { Succeeded = false, Message = message, Errors = new List<string> { error } };
}
public class Result<T>
{
    public bool Succeeded { get; set; }
    public string? Message { get; set; }
    public List<string>? Errors { get; set; }
    public T? Data { get; set; }

    public static Result<T> Success(T data, string? message = null)
    {
        return new Result<T> { Succeeded = true, Data = data, Message = message };
    }

    public static Result<T> Failure(string message, List<string>? errors = null)
    {
        return new Result<T> { Succeeded = false, Message = message, Errors = errors };
    }

    public static Result<T> Failure(string message, string error)
    {
        return new Result<T> { Succeeded = false, Message = message, Errors = new List<string> { error } };
    }
}
