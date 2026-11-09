namespace CryptoWalletAnalyzer.Services;

public static class BrowserWalletExtensions
{
    public const string MetaMask = "nkbihfbeogaeaoehlefnkodbefgpgknn";
    public const string TrustWallet = "egjidjbpglichdcondbcbdnbeeppgdph";
    public const string Phantom = "bfnaelmomeimhlpmgjnjophhpkkoljpa";
    public const string CoinbaseWallet = "hnfanknocfeofbddgcijnmhnfnkdnaad";
    public const string OkxWallet = "mcohilncbfahbmgdjkbpemcciiolgcge";
    public const string RabbyWallet = "acmacodkjbdgmoleebolmdjonilkdbch";
    public const string BinanceWallet = "fhbohimaelbohpjbbldcngcnapndodjp";
    public const string Keplr = "dmkamcknogkgcdfhhbddcghachkejeap";

    public static IReadOnlyList<BrowserWalletExtension> All { get; } =
    [
        new("MetaMask", MetaMask),
        new("Trust Wallet", TrustWallet),
        new("Phantom", Phantom),
        new("Coinbase Wallet", CoinbaseWallet),
        new("OKX Wallet", OkxWallet),
        new("Rabby Wallet", RabbyWallet),
        new("Binance Wallet", BinanceWallet),
        new("Keplr", Keplr)
    ];
}

public sealed record BrowserWalletExtension(string Name, string Id);

public static class DesktopWalletApps
{
    public const string LedgerLive = "Ledger Live";
    public const string TrezorSuite = "Trezor Suite";
    public const string Exodus = "Exodus";
    public const string Electrum = "Electrum";
    public const string AtomicWallet = "Atomic Wallet";
    public const string BitcoinCore = "Bitcoin Core";
    public const string WasabiWallet = "Wasabi Wallet";
    public const string SparrowWallet = "Sparrow Wallet";

    public static IReadOnlyList<DesktopWalletApp> All { get; } =
    [
        new("Ledger Live", LedgerLive),
        new("Trezor Suite", TrezorSuite),
        new("Exodus", Exodus),
        new("Electrum", Electrum),
        new("Atomic Wallet", AtomicWallet),
        new("Bitcoin Core", BitcoinCore),
        new("Wasabi Wallet", WasabiWallet),
        new("Sparrow Wallet", SparrowWallet)
    ];
}

public sealed record DesktopWalletApp(string Name, string Id);
