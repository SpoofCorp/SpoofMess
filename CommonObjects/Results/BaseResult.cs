namespace CommonObjects.Results;

public record class BaseResult
    (
        bool Success,
        string? Message,
        string? Error,
        int StatusCode = 200
    );