namespace RuleRoleHelper.Services;

public interface IRuleService
{
    public HasPermission HasPermission(long mask, long rule);
}
