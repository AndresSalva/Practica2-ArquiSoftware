namespace ServiceCommon.Application
{
    public class Result
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public string? Error => Errors.FirstOrDefault();
        public IReadOnlyCollection<string> Errors { get; }

        protected Result(bool isSuccess, IEnumerable<string>? errors = null)
        {
            if (isSuccess && errors?.Any() == true)
                throw new InvalidOperationException("Un resultado exitoso no puede tener errores.");

            if (!isSuccess && (errors == null || !errors.Any()))
                throw new InvalidOperationException("Un resultado fallido debe tener al menos un error.");

            IsSuccess = isSuccess;
            Errors = errors?.ToArray() ?? Array.Empty<string>();
        }

        public static Result Success() => new Result(true);

        public static Result Failure(string error) =>
            new Result(false, new[] { error });

        public static Result Failure(IEnumerable<string> errors)
        {
            var cleanErrors = errors?
                .Where(e => !string.IsNullOrWhiteSpace(e))
                .ToArray() ?? Array.Empty<string>();

            return new Result(false, cleanErrors.Length > 0
                ? cleanErrors
                : new[] { "Se produjo un error inesperado." });
        }

        public Result<T> AsResult<T>(T value) =>
            new Result<T>(value, IsSuccess, Errors);
    }

    public sealed class Result<T> : Result
    {
        public T? Value { get; }

        internal Result(T? value, bool isSuccess, IEnumerable<string>? errors = null)
            : base(isSuccess, errors)
        {
            Value = value;
        }

        public static Result<T> Success(T value) =>
            new Result<T>(value, true);

        public static new Result<T> Failure(string error) =>
            new Result<T>(default, false, new[] { error });

        public static new Result<T> Failure(IEnumerable<string> errors)
        {
            var cleanErrors = errors?
                .Where(e => !string.IsNullOrWhiteSpace(e))
                .ToArray() ?? Array.Empty<string>();

            return new Result<T>(default, false, cleanErrors.Length > 0
                ? cleanErrors
                : new[] { "Se produjo un error inesperado." });
        }
    }
}
