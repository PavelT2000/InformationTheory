using Avalonia.Controls;
using RsaLab.ViewModels;

namespace RsaLab.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        Loaded += (_, _) =>
        {
            if (DataContext is MainWindowViewModel vm)
                vm.TopLevelHost = TopLevel.GetTopLevel(this);
        };
    }
}
