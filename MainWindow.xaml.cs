namespace NoRightClickOnInputsOrTextAreaSample;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddWpfBlazorWebView(); //has to change existing ones to use this version for extension.
        serviceCollection.AddBlazorWebViewDeveloperTools();
        Resources.Add("services", serviceCollection.BuildServiceProvider());
        InitializeComponent();
    }
}