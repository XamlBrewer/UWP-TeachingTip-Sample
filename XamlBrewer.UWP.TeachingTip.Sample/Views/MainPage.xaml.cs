using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace XamlBrewer.Uwp.TeachingTip.Sample
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // PositioningTip.IsOpen = true;
            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            PositioningTip.IsOpen = false;
            base.OnNavigatingFrom(e);
        }

        private async void PositioningDemo_Clicked(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var positions = new List<TeachingTipPlacementMode>()
            {
                TeachingTipPlacementMode.Top,
                TeachingTipPlacementMode.TopRight,
                TeachingTipPlacementMode.RightTop,
                TeachingTipPlacementMode.Right,
                TeachingTipPlacementMode.RightBottom,
                TeachingTipPlacementMode.BottomRight,
                TeachingTipPlacementMode.Bottom,
                TeachingTipPlacementMode.BottomLeft,
                TeachingTipPlacementMode.LeftBottom,
                TeachingTipPlacementMode.Left,
                TeachingTipPlacementMode.LeftTop,
                TeachingTipPlacementMode.TopLeft,
                TeachingTipPlacementMode.Top,
                TeachingTipPlacementMode.Center
            };

            foreach (var position in positions)
            {
                PositioningTip.PreferredPlacement = position;
                PositioningTip.Subtitle = Regex.Replace(position.ToString(), "([a-z])([A-Z])", "$1 $2"); ;
                PositioningTip.IsOpen = true;
                await Task.Delay(750);
                PositioningTip.IsOpen = false;
                await Task.Delay(250); // You have to overcome the animation.
            }
        }
    }
}
