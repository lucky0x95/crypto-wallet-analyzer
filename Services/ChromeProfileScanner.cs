using System.IO;
using System.Text.Json;
using CryptoWalletAnalyzer.Models;

namespace CryptoWalletAnalyzer.Services;

public sealed class ChromeProfileScanner : IChromeProfileScanner
{
    private static readonly string ChromeUserDataPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "Google",
        "Chrome",
        "User Data");

    public Task<IReadOnlyList<ChromeProfile>> ScanAsync(
        IProgress<int>? progress, CancellationToken ct)
    {
        return Task.Run<IReadOnlyList<ChromeProfile>>(() =>
        {
            var profiles = new List<ChromeProfile>();
            var profileNames = ReadProfileNames();
            var options = new EnumerationOptions
            {
                IgnoreInaccessible = true,
                RecurseSubdirectories = false
            };

            if (!Directory.Exists(ChromeUserDataPath))
                throw new ChromeUserDataDirectoryNotFoundException(ChromeUserDataPath);

            foreach (var profileDirectory in Directory.EnumerateDirectories(ChromeUserDataPath, "*", options))
            {
                ct.ThrowIfCancellationRequested();

                var extensionsPath = Path.Combine(profileDirectory, "Extensions");
                if (Directory.Exists(extensionsPath))
                {
                    var folderName = Path.GetFileName(profileDirectory);
                    var displayName = profileNames.TryGetValue(folderName, out var name)
                        ? $"{folderName} ({name})"
                        : $"{folderName} (unknown)";

                    profiles.Add(new ChromeProfile(
                        folderName,
                        displayName,
                        profileDirectory));
                }

                progress?.Report(profiles.Count);
            }

            profiles.Sort((left, right) =>
                StringComparer.OrdinalIgnoreCase.Compare(left.Name, right.Name));
            progress?.Report(profiles.Count);
            return profiles;
        }, ct);
    }

    private static IReadOnlyDictionary<string, string> ReadProfileNames()
    {
        var profileNames = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        var localStatePath = Path.Combine(ChromeUserDataPath, "Local State");

        try
        {
            using var document = JsonDocument.Parse(File.ReadAllText(localStatePath));
            if (!document.RootElement.TryGetProperty("profile", out var profile) ||
                !profile.TryGetProperty("info_cache", out var infoCache) ||
                infoCache.ValueKind != JsonValueKind.Object)
            {
                return profileNames;
            }

            foreach (var profileEntry in infoCache.EnumerateObject())
            {
                if (profileEntry.Value.TryGetProperty("name", out var name) &&
                    name.ValueKind == JsonValueKind.String &&
                    !string.IsNullOrWhiteSpace(name.GetString()))
                {
                    profileNames[profileEntry.Name] = name.GetString()!;
                }
            }
        }
        catch
        {
        }

        return profileNames;
    }
}
