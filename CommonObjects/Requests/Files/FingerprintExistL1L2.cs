namespace CommonObjects.Requests.Files;

public record FingerprintExistL1L2(
        byte[] L1,
        byte[] L2,
        long FileSize
    );
