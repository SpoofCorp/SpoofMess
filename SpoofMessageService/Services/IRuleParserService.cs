using CommunicationLibrary.Communication;
using SpoofMessageService.Models.Enums;

namespace SpoofMessageService.Services;

public interface IRuleParserService
{
    public long ParseRules(Rule[] rules);
    public Rules Parse(Rule rule);
}
