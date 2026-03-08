namespace CommonObjects.Requests.Files;

public record FingerprintExistL3(
        byte[] Fingerprint,
        FileMetadata Metadata
    );