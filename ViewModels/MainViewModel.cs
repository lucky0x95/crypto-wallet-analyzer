using System.Windows.Input;
using CryptoWalletAnalyzer.Models;
using CryptoWalletAnalyzer.Services;

namespace CryptoWalletAnalyzer.ViewModels;

public sealed class MainViewModel : ViewModelBase
{
    private readonly IChromeWalletScanner _scanner;

    private IReadOnlyList<WalletProfileResult> _results = Array.Empty<WalletProfileResult>();
    private string _status = "Click Scan to check Chrome wallet extensions.";
    private bool _isBusy;
    private bool _isScanStarted;
    private bool _isProgressVisible;

    public MainViewModel(IChromeWalletScanner scanner)
    {
        _scanner = scanner;
        ScanCommand = new AsyncRelayCommand(ScanAsync);
    }

    public bool IsScanStarted
    {
        get => _isScanStarted;
        private set => Set(ref _isScanStarted, value);
    }

    public IReadOnlyList<WalletProfileResult> Results
    {
        get => _results;
        private set => Set(ref _results, value);
    }

    public string Status
    {
        get => _status;
        private set => Set(ref _status, value);
    }

    public bool IsBusy
    {
        get => _isBusy;
        private set
        {
            if (Set(ref _isBusy, value))
                CommandManager.InvalidateRequerySuggested();
        }
    }

    public bool IsProgressVisible
    {
        get => _isProgressVisible;
        private set => Set(ref _isProgressVisible, value);
    }

    public ICommand ScanCommand { get; }

    private async Task ScanAsync()
    {
        IsScanStarted = true;
        IsBusy = true;
        IsProgressVisible = true;
        Results = Array.Empty<WalletProfileResult>();
        Status = "Scanning Chrome profiles and checking wallets...";

        try
        {
            var progress = new Progress<int>(n =>
                Status = $"Checking wallet profiles... {n:N0} rows");
            Results = await _scanner.ScanAsync(progress, CancellationToken.None);
            Status = Results.Count == 0
                ? "No Chrome profiles with an Extensions folder were found."
                : $"Checked {Results.Count:N0} wallet/profile rows.";
        }
        catch (OperationCanceledException)
        {
            Status = "Scan cancelled.";
        }
        catch (ChromeUserDataDirectoryNotFoundException)
        {
            Status = "You didn't installed google chrome";
        }
        catch (Exception ex)
        {
            Status = $"Scan failed: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
            await Task.Delay(300);
            IsProgressVisible = false;
        }
    }
}
