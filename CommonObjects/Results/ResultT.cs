using System.ComponentModel;

namespace CommonObjects.Results;

public class Result<T>
{
    public bool Success { get; set; }

    public string? Message { get; set; }

    public string? Error { get; set; }


    public int StatusCode = 200;

    public T? Body { get; set; }

    private static Result<T> GetResult(bool success = true, string? message = null, string? error = null, int statusCode = 200, T? body = default) =>
     new()
     {
         Success = success,
         Error = error,
         StatusCode = statusCode,
         Message = message,
         Body = body,
     };

    [Obsolete("Используйте специализированные методы.")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Result<T> SuccessResult(T body, string? message = "OK", int statusCode = 200) =>
        GetResult(success: true, message: message, statusCode: statusCode, body: body);

    #pragma warning disable CS0618
    public static Result<T> OkResult(T body) =>
        SuccessResult(body);
    #pragma warning restore CS0618
    public static Result<T> NotFoundResult(string error) =>
        ErrorResult(error, 404);

    public static Result<T> Forbidden(string error = "Forbidden") =>
        ErrorResult(error, 403);
    public static Result<T> BadRequest(string error) =>
        ErrorResult(error, 400);

    public static Result<T> ErrorResult(string error, int statusCode = 500) =>
        GetResult(success: false, error: error, statusCode: statusCode);

    public static Result<T> From(Result result) =>
        GetResult(success: result.Success, message: result.Message, error: result.Error, statusCode: result.StatusCode);
    public static Result<T> From<TAnother>(Result<TAnother> result) =>
        GetResult(success: result.Success, message: result.Message, error: result.Error, statusCode: result.StatusCode);
}
