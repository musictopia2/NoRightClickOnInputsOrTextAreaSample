using Microsoft.AspNetCore.Components.WebView;
using System.Diagnostics;

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

        bwv.BlazorWebViewInitialized += Bwv_Initialized;
    }

    private void Bwv_Initialized(object? sender, BlazorWebViewInitializedEventArgs e)
    {
        e.WebView.CoreWebView2.Settings.AreDefaultContextMenusEnabled = true;
        e.WebView.CoreWebView2.ContextMenuRequested += CoreWebView2_ContextMenuRequested;
    }

    private void CoreWebView2_ContextMenuRequested(object? sender, Microsoft.Web.WebView2.Core.CoreWebView2ContextMenuRequestedEventArgs e)
    {
        Debug.WriteLine($"IsEditable? {e.ContextMenuTarget.IsEditable}");

        if (e.ContextMenuTarget.IsEditable)
        {
            // For editable elements such as <input> and <textarea> we enable the context menu but remove items we don't want in this app
            var itemNamesToRemove = new[] { "share", "webSelect", "webCapture", "inspectElement" };
            var menuIndexesToRemove =
                e.MenuItems
                    .Select((m, i) => (m, i))
                    .Where(m => itemNamesToRemove.Contains(m.m.Name))
                    .Select(m => m.i)
                    .Reverse();

            Debug.WriteLine($"Removing these indexes: {string.Join(", ", menuIndexesToRemove.Select(i => i.ToString()))}");
            foreach (var menuIndexToRemove in menuIndexesToRemove)
            {
                Debug.WriteLine($"Removing {e.MenuItems[menuIndexToRemove].Name}...");
                e.MenuItems.RemoveAt(menuIndexToRemove);
            }

            // Trim extra separators from the end
            while (e.MenuItems.Last().Kind ==  Microsoft.Web.WebView2.Core.CoreWebView2ContextMenuItemKind.Separator)
            {
                e.MenuItems.RemoveAt(e.MenuItems.Count - 1);
            }
        }
        else
        {
            // For non-editable elements such as <div> we disable the context menu
            e.Handled = true;
        }
    }
}
