using Mvvm.Services;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace XamlBrewer.Uwp.TeachingTip.Sample
{
    public sealed partial class HomePage : Page
    {
        public HomePage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Displays the Main menu teaching tip again.
        /// </summary>
        private void ReplayButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ContextualInformation.DisplayMainMenuTip();
        }

        /// <summary>
        /// Reset to factory settings.
        /// </summary>
        private void ResetButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var containerSettings = (ApplicationDataContainerSettings)ApplicationData.Current.LocalSettings.Values;
            var keys = containerSettings.Keys;
            foreach (var key in keys)
            {
                ApplicationData.Current.LocalSettings.Values.Remove(key);
            }

            ResetButtonTeachingTip.IsOpen = true;
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            ResetButtonTeachingTip.IsOpen = false;

            base.OnNavigatingFrom(e);
        }
    }
}
