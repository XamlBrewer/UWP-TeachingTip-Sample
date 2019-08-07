using Mvvm.Services;
using Windows.UI.Xaml.Controls;

namespace XamlBrewer.Uwp.TeachingTip.Sample
{
    public sealed partial class HomePage : Page
    {
        public HomePage()
        {
            this.InitializeComponent();
        }

        private void ReplayButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ContextualInformation.DisplayMainMenuTip();
        }
    }
}
