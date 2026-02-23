using CommunicationLibrary.Communication;
using RuleRoleHelper;
using SpoofMessageService.Models.Enums;
using SpoofMessageService.Services;

namespace SpoofMessageService.ServiceRealizations;

public class RuleParserService : IRuleParserService
{
    public long ParseRules(Rule?[] rules)
    {
        long result = 0;
        Rules rule;
        if (rules is null)
            return result;
        for (int i = 0; i < rules.Length; i++)
        {
            rule = Parse(rules[i]);
            result |= (long)rule;
        }
        return result;
    }

    public Rules Parse(Rule rule)
    {
        return (Permissions)rule.PermissionId switch
        {
            Permissions.SendTexts => Rules.SendTexts,
            Permissions.SendAudios => Rules.SendAudios,
            Permissions.SendVideos => Rules.SendVideos,
            Permissions.SendFiles => Rules.SendFiles,
            Permissions.SendEmoji => Rules.SendEmoji,
            Permissions.SendSticker => Rules.SendSticker,
            Permissions.SendVoiceMessage => Rules.SendVoiceMessage,
            Permissions.SendVideoMessage => Rules.SendVideoMessage,
            Permissions.ShareMessage => Rules.ShareMessage,
            Permissions.DeleteMessage => Rules.DeleteMessage,
            Permissions.EditMessage => Rules.EditMessage,
            _ => throw new ArgumentOutOfRangeException($"Permission with id {rule.PermissionId} is not supported.")
        };
    }
}
