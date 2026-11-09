namespace CryptoWalletAnalyzer.Models;

public sealed record WalletProfileResult(
    string Wallet,
    string ExtensionName,
    string ExtensionId,
    string Status,
    string InstallDate,
    string Browser,
    string Profile);
