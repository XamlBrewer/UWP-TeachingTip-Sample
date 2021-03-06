﻿using Microsoft.UI.Xaml.Controls;
using Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
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

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            var openPopups = VisualTreeHelper.GetOpenPopups(Window.Current);
            foreach (var popup in openPopups)
            {
                popup.IsOpen = false;
            }

            // Alternative:
            // VisualTreeHelper.GetOpenPopups(Window.Current).ToList().ForEach(p => p.IsOpen = false);

            // This tip continuously flipflops, so needs its own call.
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
                Content = "Don't worry, you will soon feel better.",
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

        private void WizardButton_Clicked(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            WizardList.SelectedIndex = 0;
            WizardTip.IsOpen = true;
        }

        private void WizardTip_ActionButtonClick(Microsoft.UI.Xaml.Controls.TeachingTip sender, object args)
        {
            if (WizardList.SelectedIndex > -1)
            {
                WizardField.Text = (WizardList.SelectedItem as TextBlock).Text;
            }

            WizardTip.IsOpen = false;
        }
    }
}
