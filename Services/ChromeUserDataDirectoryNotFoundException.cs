namespace CryptoWalletAnalyzer.Services;

public sealed class ChromeUserDataDirectoryNotFoundException : Exception
{
    public ChromeUserDataDirectoryNotFoundException(string path)
        : base($"Chrome user data directory was not found: {path}")
    {
    }
}
