    namespace GYMPT.Domain.Shared
    {
        public class Result
        {
            public bool IsSuccess { get; private set; }
            public string Error { get; private set; } = string.Empty;

            public bool IsFailure => !IsSuccess;

            // Constructor privado para forzar uso de métodos de creación
            private Result(bool isSuccess, string error)
            {
                IsSuccess = isSuccess;
                Error = error;
            }

            // Retorna un resultado exitoso
            public static Result Ok() => new Result(true, string.Empty);

            // Retorna un resultado fallido con mensaje
            public static Result Fail(string error) => new Result(false, error);
        }
    }
