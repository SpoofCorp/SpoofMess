using CommonObjects.Results;
using SpoofEntranceService.Models;
using SpoofEntranceService.ServiceRealizations.Validators;

namespace SpoofEntranceServiceTest.SessionServiceTests;

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

    public static IEnumerable<object[]> GetSessionInvalidInfos()
    {
        yield return new object[] { null!, 404 };
        yield return new object[] { new SessionInfo { IsActive = false }, 400 };
        yield return new object[] { new SessionInfo { IsDeleted = true }, 400 };
        yield return new object[] { new SessionInfo { IsActive = false, IsDeleted = true }, 400 };
        yield return new object[] { new SessionInfo { IsActive = true, IsDeleted = true }, 400 };
        yield return new object[] { new SessionInfo { IsActive = true, IsDeleted = false }, 200 };
    }

    public static IEnumerable<object[]> GetTrustSessionInfos()
    {
        yield return new object[] { null!, 404 };
        yield return new object[] { new SessionInfo { IsActive = false }, 400 };
        yield return new object[] { new SessionInfo { IsActive = false, CreatedAt = DateTime.UtcNow }, 400 };
        yield return new object[] { new SessionInfo { IsDeleted = true }, 400 };
        yield return new object[] { new SessionInfo { IsDeleted = true, CreatedAt = DateTime.UtcNow }, 400 };
        yield return new object[] { new SessionInfo { IsActive = false, IsDeleted = true }, 400 };
        yield return new object[] { new SessionInfo { IsActive = false, IsDeleted = true, CreatedAt = DateTime.UtcNow }, 400 };
        yield return new object[] { new SessionInfo { IsActive = true, IsDeleted = true }, 400 };
        yield return new object[] { new SessionInfo { IsActive = true, IsDeleted = true, CreatedAt = DateTime.UtcNow }, 400 };
        yield return new object[] { new SessionInfo { IsActive = true, IsDeleted = false , CreatedAt = DateTime.UtcNow.AddSeconds(-1) }, 403 };
        yield return new object[] { new SessionInfo { IsActive = true, IsDeleted = false , CreatedAt = DateTime.UtcNow.AddDays(-2) }, 403 };
        yield return new object[] { new SessionInfo { IsActive = true, IsDeleted = false , CreatedAt = DateTime.UtcNow.AddDays(-4) }, 403 };
        yield return new object[] { new SessionInfo { IsActive = true, IsDeleted = false , CreatedAt = DateTime.UtcNow.AddDays(-7) }, 403 };
        yield return new object[] { new SessionInfo { IsActive = true, IsDeleted = false , CreatedAt = DateTime.UtcNow.AddDays(-8) }, 200 };
    }

    public static IEnumerable<object[]> GetAgesSessionInfos()
    {
        yield return new object[] { new SessionInfo { CreatedAt = DateTime.UtcNow }, true };
        yield return new object[] { new SessionInfo { CreatedAt = DateTime.UtcNow.AddDays(-4) }, true };
        yield return new object[] { new SessionInfo { CreatedAt = DateTime.UtcNow.AddDays(-7) }, true };
        yield return new object[] { new SessionInfo { CreatedAt = DateTime.UtcNow.AddDays(-8) }, false };
        yield return new object[] { new SessionInfo { CreatedAt = DateTime.UtcNow.AddDays(-10) }, false };
    }
}
