using CryptoWalletAnalyzer.Models;

namespace CryptoWalletAnalyzer.Services;

public interface IChromeWalletScanner
{
    Task<IReadOnlyList<WalletProfileResult>> ScanAsync(
        IProgress<int>? progress, CancellationToken ct);
}
