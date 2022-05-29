using System.Windows;
using AdonisUI;

using XrefViewerUI.Core;
using XrefViewerUI.Core.Network;

namespace XrefViewerUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            ResourceLocator.SetColorScheme(Current.Resources, ResourceLocator.DarkColorScheme);
            DataHandler.Init();
            NetworkManager.Init();
            Current.MainWindow = new MainWindow();
            Current.MainWindow.Show();
        }
    }
}
