using CommonObjects.Results;
using SpoofEntranceService.Models;
using SpoofEntranceService.ServiceRealizations.Validators;

namespace SpoofEntranceServiceTest.ValidatorsTests;

public class SessionServiceTests
{
    [Theory]
    [MemberData(nameof(GetSessionInvalidInfos))]
    public void Check_validate_session_info(SessionInfo session, int statusCode)
    {
        SessionValidator sut = new();

        Result result = sut.IsInvalidSession(session);

        Assert.Equal(statusCode, result.StatusCode);
    }

    [Theory]
    [MemberData(nameof(GetAgesSessionInfos))]
    public void Validate_session_creation_time(SessionInfo session, bool realResult)
    {
        SessionValidator sut = new();

        bool result = sut.IsSessionTooNew(session, DateTime.UtcNow);

        Assert.Equal(realResult, result);
    }

    [Theory]
    [MemberData(nameof(GetTrustSessionInfos))]
    public void Validate_trust_session_infos(SessionInfo session, int statusCode)
    {
        SessionValidator sut = new();

        Result result = sut.ValidateTrustSession(session);

        Assert.Equal(statusCode, result.StatusCode);
    }

    public static TheoryData<SessionInfo, int> GetSessionInvalidInfos()
    {
        TheoryData<SessionInfo, int> data = [];
        data.Add(null!, 404);
        data.Add(new SessionInfo { IsActive = false }, 400);
        data.Add(new SessionInfo { IsDeleted = true }, 400);
        data.Add(new SessionInfo { IsActive = false, IsDeleted = true }, 400);
        data.Add(new SessionInfo { IsActive = true, IsDeleted = true }, 400);
        data.Add(new SessionInfo { IsActive = true, IsDeleted = false }, 200);
        return data;
    }

    public static TheoryData<SessionInfo, int> GetTrustSessionInfos()
    {
        TheoryData<SessionInfo, int> data = [];
        data.Add(null!, 404);
        data.Add(new SessionInfo { IsActive = false }, 400);
        data.Add(new SessionInfo { IsActive = false, CreatedAt = DateTime.UtcNow }, 400);
        data.Add(new SessionInfo { IsDeleted = true }, 400);
        data.Add(new SessionInfo { IsDeleted = true, CreatedAt = DateTime.UtcNow }, 400);
        data.Add(new SessionInfo { IsActive = false, IsDeleted = true }, 400);
        data.Add(new SessionInfo { IsActive = false, IsDeleted = true, CreatedAt = DateTime.UtcNow }, 400);
        data.Add(new SessionInfo { IsActive = true, IsDeleted = true }, 400);
        data.Add(new SessionInfo { IsActive = true, IsDeleted = true, CreatedAt = DateTime.UtcNow }, 400);
        data.Add(new SessionInfo { IsActive = true, IsDeleted = false, CreatedAt = DateTime.UtcNow.AddSeconds(-1) }, 403);
        data.Add(new SessionInfo { IsActive = true, IsDeleted = false, CreatedAt = DateTime.UtcNow.AddDays(-2) }, 403);
        data.Add(new SessionInfo { IsActive = true, IsDeleted = false, CreatedAt = DateTime.UtcNow.AddDays(-4) }, 403);
        data.Add(new SessionInfo { IsActive = true, IsDeleted = false, CreatedAt = DateTime.UtcNow.AddDays(-7) }, 403);
        data.Add(new SessionInfo { IsActive = true, IsDeleted = false, CreatedAt = DateTime.UtcNow.AddDays(-8) }, 200);
        return data;
    }

    public static TheoryData<SessionInfo, bool> GetAgesSessionInfos()
    {
        TheoryData<SessionInfo, bool> data = [];
        data.Add(new SessionInfo { CreatedAt = DateTime.UtcNow }, true);
        data.Add(new SessionInfo { CreatedAt = DateTime.UtcNow.AddDays(-4) }, true);
        data.Add(new SessionInfo { CreatedAt = DateTime.UtcNow.AddDays(-7) }, true);
        data.Add(new SessionInfo { CreatedAt = DateTime.UtcNow.AddDays(-8) }, false);
        data.Add(new SessionInfo { CreatedAt = DateTime.UtcNow.AddDays(-10) }, false);
        return data;
    }
}
