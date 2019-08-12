using Microsoft.UI.Xaml.Controls;
using System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using XamlBrewer.Uwp.TeachingTip.Sample;

namespace Mvvm.Services
{
    public static partial class ContextualInformation
    {
        private static TeachingTip _replayButtonTeachingTip;

        /// <summary>
        /// Displays the TeachingTip for the Replay button.
        /// </summary>
        public static void DisplayReplayButtonTip()
        {
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

            if (localSettings.Values["replayButtonTeachingTipDisplayed"] != null && 
                localSettings.Values["replayButtonTeachingTipDisplayed"].ToString() == "True")
            {
                return;
            }

            // Find the Main menu item and the Content grid.
            var frame = Navigation.Frame;
            var homePage = frame?.Content as HomePage;
            var replayButton = homePage?.FindName("ReplayButton") as Button;

            _replayButtonTeachingTip = new TeachingTip
            {
                Target = replayButton,
                Title = "Click to replay",
                Content = "Don't worry, you'll never see this tip again. " + Environment.NewLine + "Promised! 🤞",
                HeroContent = new Image
                {
                    Source = new BitmapImage(new Uri("ms-appx:///Assets/FriendsDontLie.jpg"))
                },
                PreferredPlacement = TeachingTipPlacementMode.Right, // Ignored since there's not enough space.
                IsOpen = true,
                BorderThickness = new Thickness(.5),
                BorderBrush = new SolidColorBrush(Colors.DarkRed)
            };

            localSettings.Values["replayButtonTeachingTipDisplayed"] = "True";

            (homePage.Content as Grid).Children.Add(_replayButtonTeachingTip);
        }
    }
}
