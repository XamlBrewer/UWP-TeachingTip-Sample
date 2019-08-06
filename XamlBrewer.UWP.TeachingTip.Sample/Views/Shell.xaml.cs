using System.Diagnostics;
using Mvvm;
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using Mvvm.Services;
using Windows.UI.ViewManagement;
using Windows.ApplicationModel.Core;
using Windows.UI.Xaml.Media;
using System;

namespace XamlBrewer.Uwp.TeachingTip.Sample
{
    public sealed partial class Shell
    {
        public Shell()
        {
            // Blends the app into the title bar.
            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;

            this.InitializeComponent();

            // Update the title bar when the back button (dis)appears or resizes.
            Window.Current.CoreWindow.SizeChanged += (s, e) => UpdateAppTitle();
            coreTitleBar.LayoutMetricsChanged += (s, e) => UpdateAppTitle();

            Window.Current.SetTitleBar(AppTitleBar);

            // Initialize Navigation Service.
            Navigation.Frame = SplitViewFrame;

            Loaded += Shell_Loaded;
        }

        private Microsoft.UI.Xaml.Controls.TeachingTip _teachingTip;

        private void Shell_Loaded(object sender, RoutedEventArgs e)
        {
            var mainPageMenu = Menu.ContainerFromIndex(1) as ListViewItem;
            _teachingTip = new Microsoft.UI.Xaml.Controls.TeachingTip
            {
                Target = mainPageMenu,
                Title = "Welcome",
                Subtitle = "This is where the action is.",
                PreferredPlacement = Microsoft.UI.Xaml.Controls.TeachingTipPlacementMode.BottomRight,
                IsOpen = true,
                BorderThickness = new Thickness(.5),
                BorderBrush = new SolidColorBrush(Colors.DarkRed)
            };
            ContentGrid.Children.Add(_teachingTip);
            var timer = new DispatcherTimer();
            timer.Tick += Timer_Tick;
            timer.Interval = TimeSpan.FromSeconds(5);
            timer.Start();
        }

        private void Timer_Tick(object sender, object e)
        {
            (sender as DispatcherTimer).Stop();
            _teachingTip.IsOpen = false;
        }

        /// <summary>
        /// Updates the title bar when the back button (dis)appears or resizes.
        /// </summary>
        private void UpdateAppTitle()
        {
            var full = (ApplicationView.GetForCurrentView().IsFullScreenMode);
            var left = (full ? 0 : CoreApplication.GetCurrentView().TitleBar.SystemOverlayLeftInset);
            HamburgerButton.Margin = new Thickness(left, 0, 0, 0);
            AppTitle.Margin = new Thickness(left + HamburgerButton.Width + 12, 8, 0, 0);
        }

        // Navigate to another page.
        private void Menu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                // Unselect the other menu.
                if ((sender as ListView) == Menu)
                {
                    SecondMenu.SelectedItem = null;
                }
                else
                {
                    Menu.SelectedItem = null;
                }

                if (e.AddedItems.First() is MenuItem menuItem && menuItem.IsNavigation)
                {
                    Navigation.Navigate(menuItem.NavigationDestination);
                }
            }
        }

        // Execute command.
        private void Menu_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is MenuItem menuItem && !menuItem.IsNavigation)
            {
                Debugger.Break();
                menuItem.Command.Execute(null);
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Navigation.EnableBackButton();

            // Navigate to home page.
            Navigation.Navigate(typeof(HomePage), e.Parameter);

            base.OnNavigatedTo(e);
        }

        // Swipe to open the splitview panel.
        private void SplitViewOpener_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            if (e.Cumulative.Translation.X > 50)
            {
                ShellSplitView.IsPaneOpen = true;
            }
        }

        // Swipe to close the splitview panel.
        private void SplitViewPane_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            if (e.Cumulative.Translation.X < -50)
            {
                ShellSplitView.IsPaneOpen = false;
            }
        }

        // Open or close the splitview panel through Hamburger button.
        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            ShellSplitView.IsPaneOpen = !ShellSplitView.IsPaneOpen;
        }

        private void SplitViewFrame_OnNavigated(object sender, NavigationEventArgs e)
        {
            // Lookup destination type in menu(s)
            var item = (from i in Menu.Items
                        where (i as MenuItem).NavigationDestination == e.SourcePageType
                        select i).FirstOrDefault();
            if (item != null)
            {
                Menu.SelectedItem = item;
                return;
            }

            Menu.SelectedIndex = -1;

            item = (from i in SecondMenu.Items
                    where (i as MenuItem).NavigationDestination == e.SourcePageType
                    select i).FirstOrDefault();
            if (item != null)
            {
                SecondMenu.SelectedItem = item;
                return;
            }

            SecondMenu.SelectedIndex = -1;
        }
    }
}
