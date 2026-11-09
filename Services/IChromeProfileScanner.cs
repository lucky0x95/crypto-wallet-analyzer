using CryptoWalletAnalyzer.Models;

namespace CryptoWalletAnalyzer.Services;

public interface IChromeProfileScanner
{
    Task<IReadOnlyList<ChromeProfile>> ScanAsync(
        IProgress<int>? progress, CancellationToken ct);
}
