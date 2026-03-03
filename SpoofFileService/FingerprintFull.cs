using SecurityLibrary;

namespace SpoofFileService;

public record struct FingerprintFull(
        byte[] L1,
        byte[] L2,
        FileResult FileResult
    );
