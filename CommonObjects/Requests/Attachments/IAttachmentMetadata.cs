using System.Text.Json.Serialization;

namespace CommonObjects.Requests.Attachments;

[JsonDerivedType(typeof(VoiceMessageMetadata), "voiceMessage")]
public interface IAttachmentMetadata
{

}
