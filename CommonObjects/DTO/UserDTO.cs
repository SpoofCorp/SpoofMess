namespace CommonObjects.DTO;

public record UserDTO(
        Guid Id,
        string Name,
        string Login,
        byte[]? FingerPrintAvatar
    );
