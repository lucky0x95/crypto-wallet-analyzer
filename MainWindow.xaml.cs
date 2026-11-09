using System.Windows;
using CryptoWalletAnalyzer.Services;
using CryptoWalletAnalyzer.ViewModels;

namespace CryptoWalletAnalyzer;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainViewModel(
            new ChromeWalletScanner(new ChromeProfileScanner()));
    }
}
