namespace CommonObjects.Requests.Files;

public record FingerprintExistL1(byte[] FirstMb, long FileSize);
public record FingerprintExist(byte[] Fingerprint, long FileSize);
