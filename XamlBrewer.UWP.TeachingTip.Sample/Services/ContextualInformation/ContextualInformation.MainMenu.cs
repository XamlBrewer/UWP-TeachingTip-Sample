using Microsoft.UI.Xaml.Controls;
using System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Mvvm.Services
{
    public static partial class ContextualInformation
    {
        private static TeachingTip _mainMenuTeachingTip;

        /// <summary>
        /// Displays the TeachingTip for the Main menu item, and hides it after 5 seconds.
        /// </summary>
        public static void DisplayMainMenuTip()
        {
            // Find the Main menu item and the Content grid.
            var shell = (Window.Current.Content as Frame)?.Content as FrameworkElement;
            var contentGrid = shell?.FindName("ContentGrid") as Grid;
            var menu = shell?.FindName("Menu") as ListView;
            var mainPageMenu = menu?.ContainerFromIndex(1) as ListViewItem;

            // The menu item itself would make a good target for the teaching tip,
            // but let's be ambitious and dive deeper to find its icon.
            var itemsPresenter = ((VisualTreeHelper.GetChild(mainPageMenu, 0)) as FrameworkElement);
            var stackPanel = ((VisualTreeHelper.GetChild(itemsPresenter, 0)) as FrameworkElement);
            var glyph = stackPanel?.FindName("Glyph") as FrameworkElement;

            _mainMenuTeachingTip = new TeachingTip
            {
                Target = glyph,
                Title = "Welcome",
                Subtitle = "The Main page is where all the action is.",
                HeroContent = new Image
                {
                    Source = new BitmapImage(new Uri("ms-appx:///Assets/MindFlayer.jpg"))
                },
                PreferredPlacement = TeachingTipPlacementMode.BottomRight,
                IsOpen = true,
                BorderThickness = new Thickness(.5),
                BorderBrush = new SolidColorBrush(Colors.DarkRed)
            };
            _mainMenuTeachingTip.Closed += MainMenuTeachingTip_Closed;

            contentGrid.Children.Add(_mainMenuTeachingTip);
            var timer = new DispatcherTimer();
            timer.Tick += MainMenuTimer_Tick;
            timer.Interval = TimeSpan.FromSeconds(5);
            timer.Start();
        }

        private static void MainMenuTeachingTip_Closed(TeachingTip sender, TeachingTipClosedEventArgs args)
        {
            if (_mainMenuTeachingTip != null)
            {
                _mainMenuTeachingTip.Closed -= MainMenuTeachingTip_Closed;
            }

            DisplayReplayButtonTip();
        }

        private static void MainMenuTimer_Tick(object sender, object e)
        {
            (sender as DispatcherTimer).Stop();

            if (_mainMenuTeachingTip == null)
            {
                return;
            }

            // Close and cleanup the TeachingTip
            _mainMenuTeachingTip.IsOpen = false;
            (_mainMenuTeachingTip.Parent as Grid).Children.Remove(_mainMenuTeachingTip);
            // _mainMenuTeachingTip.Closed -= MainMenuTeachingTip_Closed; --> too early
            _mainMenuTeachingTip = null;
        }
    }
}
