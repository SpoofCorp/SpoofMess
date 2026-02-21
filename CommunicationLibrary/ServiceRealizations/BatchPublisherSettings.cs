namespace CommunicationLibrary.ServiceRealizations;

public record BatchPublisherSettings(
        int QueueCount,
        int BatchCount,
        int BatchLifeTime,
        int BatchTimeNewMessage
    );
