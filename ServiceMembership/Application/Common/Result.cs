using System;
using System.Collections.Generic;
using System.Linq;

namespace ServiceMembership.Application.Common;

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public string? Error { get; }
    public IReadOnlyCollection<string> Errors { get; }

    protected Result(bool isSuccess, IReadOnlyCollection<string> errors)
    {
        IsSuccess = isSuccess;
        Errors = errors;
        Error = errors.FirstOrDefault();
    }

    public static Result Success() => new Result(true, Array.Empty<string>());

    public static Result Failure(string error) => new Result(false, new[] { error });

    public static Result Failure(IEnumerable<string> errors)
    {
        var errorList = errors?
            .Where(e => !string.IsNullOrWhiteSpace(e))
            .ToArray() ?? Array.Empty<string>();

        return new Result(false, errorList.Length > 0 ? errorList : new[] { "Se produjo un error inesperado." });
    }

    public Result<T> AsResult<T>(T value) => new Result<T>(IsSuccess, Errors, value);
}

public sealed class Result<T> : Result
{
    public T? Value { get; }

    internal Result(bool isSuccess, IReadOnlyCollection<string> errors, T? value)
        : base(isSuccess, errors)
    {
        Value = value;
    }

    public static Result<T> Success(T value) => new Result<T>(true, Array.Empty<string>(), value);

    public static new Result<T> Failure(string error) => new Result<T>(false, new[] { error }, default);

    public static new Result<T> Failure(IEnumerable<string> errors)
    {
        var errorList = errors?
            .Where(e => !string.IsNullOrWhiteSpace(e))
            .ToArray() ?? Array.Empty<string>();

        return new Result<T>(false, errorList.Length > 0 ? errorList : new[] { "Se produjo un error inesperado." }, default);
    }
}
