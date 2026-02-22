namespace CommonObjects.Requests.Files;

public record FingerprintExistL2(byte[] FirstMb, byte[] CenterMb, byte[] LastMb, long FileSize);
