using CommonObjects.Requests.Files;

namespace CommonObjects.DTO;

public record class StickerDTO(
        Guid FileId,
        Guid Id,
        string Name
    );