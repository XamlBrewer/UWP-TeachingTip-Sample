using Microsoft.UI.Xaml.Controls;
using Mvvm;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using XamlBrewer.UWP.Controls;

namespace XamlBrewer.Uwp.TeachingTip.Sample
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private DelegateCommand ActionCommand => new DelegateCommand(Action_Executed);
        private DelegateCommand CloseCommand => new DelegateCommand(Close_Executed);

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            PositioningTip.IsOpen = false;
            base.OnNavigatingFrom(e);
        }

        private async void PositioningDemo_Clicked(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            PositioningTip.Title = "Targeted position";
            PositioningTip.Target = PositioningButton;
            await MoveAround();

            PositioningTip.Title = "Untargeted position";
            PositioningTip.Target = null;
            await MoveAround();
        }

        private async Task MoveAround()
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

        private void LightDismissButton_Clicked(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            LightDismissTip.IsOpen = true;
        }

        private void ButtonsButton_Clicked(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ButtonsTip.IsOpen = true;
        }

        private void Action_Executed()
        {
            var _actionTeachingTip = new AutoCloseTeachingTip
            {
                Title = "Well, eat this then.",
                Content = "Don't worry, it will soon be over",
                HeroContent = new Image
                {
                    Source = new BitmapImage(new Uri("ms-appx:///Assets/Heather.jpg"))
                },
                PreferredPlacement = TeachingTipPlacementMode.Right,
                IsOpen = true
            };

            (Content as Grid).Children.Add(_actionTeachingTip);
        }

        private void Close_Executed()
        {
            var _closeTeachingTip = new AutoCloseTeachingTip
            {
                Title = "Then what are you still doing there?",
                Content = "We are all waiting for you in the factory.",
                HeroContent = new Image
                {
                    Source = new BitmapImage(new Uri("ms-appx:///Assets/BrimbornSteelworks.png"))
                },
                PreferredPlacement = TeachingTipPlacementMode.Right,
                IsOpen = true
            };

            (Content as Grid).Children.Add(_closeTeachingTip);
        }
    }
}
