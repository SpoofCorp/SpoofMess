namespace CommonObjects.DTO;

public record UserDTO(
        Guid Id,
        string Name,
        string Login,
        Guid? FileId
    );
