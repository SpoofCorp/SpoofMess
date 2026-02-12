using RuleRoleHelper.Services;

namespace RuleRoleHelper.ServiceRealizations;

public class RuleService : IRuleService
{
    public HasPermission HasPermission(long mask, long rule) =>
        (mask & rule) == rule ? RuleRoleHelper.HasPermission.Allow : RuleRoleHelper.HasPermission.Deny;
}
