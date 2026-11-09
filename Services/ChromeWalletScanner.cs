using System.Globalization;
using System.IO;
using CryptoWalletAnalyzer.Models;

namespace CryptoWalletAnalyzer.Services;

public sealed class ChromeWalletScanner : IChromeWalletScanner
{
    private readonly IChromeProfileScanner _profileScanner;

    public ChromeWalletScanner(IChromeProfileScanner profileScanner)
    {
        _profileScanner = profileScanner;
    }

    public async Task<IReadOnlyList<WalletProfileResult>> ScanAsync(
        IProgress<int>? progress, CancellationToken ct)
    {
        var profiles = await _profileScanner.ScanAsync(progress, ct).ConfigureAwait(false);

        return await Task.Run<IReadOnlyList<WalletProfileResult>>(() =>
        {
            var results = new List<WalletProfileResult>(
                profiles.Count * BrowserWalletExtensions.All.Count);
            var completed = 0;

            foreach (var profile in profiles)
            {
                foreach (var wallet in BrowserWalletExtensions.All)
                {
                    ct.ThrowIfCancellationRequested();

                    var idFolder = Path.Combine(profile.Path, "Extensions", wallet.Id);
                    var installed = Directory.Exists(idFolder);
                    var installDate = string.Empty;

                    if (installed)
                    {
                        try
                        {
                            DateTime installedAt = new DirectoryInfo(idFolder).CreationTime;
                            installDate = installedAt.ToString(
                                "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            installDate = "Unknown";
                        }
                    }

                    results.Add(new WalletProfileResult(
                        Wallet: wallet.Name,
                        ExtensionName: wallet.Name,
                        ExtensionId: wallet.Id,
                        Status: installed ? "Installed" : "Not installed",
                        InstallDate: installDate,
                        Browser: "Google Chrome",
                        Profile: profile.DisplayName));

                    progress?.Report(++completed);
                }
            }

            return results;
        }, ct).ConfigureAwait(false);
    }
}
