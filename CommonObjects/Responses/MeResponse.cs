using CommonObjects.DTO;

namespace CommonObjects.Responses;

public record class MeResponse(
    UserAuthorizeResponse UserAuthorizeResponse,
    UserDTO UserDTO);