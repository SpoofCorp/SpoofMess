namespace CommonObjects.Results;

public record class BaseResult
{
    public bool Success { get; set; }

    public string? Message { get; set; }

    public string? Error { get; set; }

    public int StatusCode { get; set; } = 200;
}
