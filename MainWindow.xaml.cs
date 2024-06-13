#pragma warning disable IDE0090 // Use 'new(...)'

using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AniFlow_.NET
{
    public partial class MainWindow : Window
    {
        private string[] LibrarySearchArguments = [];
        private static readonly char[] LibrarySearchArgumentSeperator = [' '];

        public MainWindow()
        {
            InitializeComponent();

            if (!Directory.Exists("./Library"))
                Directory.CreateDirectory("./Library");
        }

        public class InitLayout
        {
            public required string Name { get; set; }
            public string[]? Aliases { get; set; }
            public string? Author { get; set; }
            public string? IMDB_Link { get; set; }
            public required bool IsValidRegistry {  get; set; }
        }

        internal class MDL2FontIcons
        {
            public static string RestoreIcon = "\xE923";
            public static string MaximizeIcon = "\xE922";
            public static string NavigationIcon = "\xE700";
            public static string LibraryIcon = "\xE1D3";
            public static string GearIcon = "\xE713";
            public static string AddIcon = "\xE710";
        }

        internal class Animations
        {
            public static DoubleAnimation NavigationBar__150 = new DoubleAnimation
            {
                To = 150,
                Duration = new Duration(TimeSpan.FromMilliseconds(350)),
                EasingFunction = new CubicEase()
            };

            public static DoubleAnimation NavigationBar__50 = new DoubleAnimation
            {
                To = 50,
                Duration = new Duration(TimeSpan.FromMilliseconds(350)),
                EasingFunction = new CubicEase()
            };

            public static ThicknessAnimation ContentPresenter__149_40_0_0 = new ThicknessAnimation
            {
                To = new Thickness(149, 40, 0, 0),
                Duration = new Duration(TimeSpan.FromMilliseconds(350)),
                EasingFunction = new CubicEase()
            };

            public static ThicknessAnimation ContentPresenter__49_40_0_0 = new ThicknessAnimation
            {
                To = new Thickness(49, 40, 0, 0),
                Duration = new Duration(TimeSpan.FromMilliseconds(350)),
                EasingFunction = new CubicEase()
            };

            public static DoubleAnimation NavigationButton__0 = new DoubleAnimation
            {
                To = 0,
                Duration = new Duration(TimeSpan.FromMilliseconds(350)),
                EasingFunction = new CubicEase()
            };

            public static DoubleAnimation NavigationButton__1 = new DoubleAnimation
            {
                To = 1,
                Duration = new Duration(TimeSpan.FromMilliseconds(350)),
                EasingFunction = new CubicEase()
            };

            public static DoubleAnimation LibrarySwitchButton__0 = new DoubleAnimation
            {
                To = 0,
                Duration = new Duration(TimeSpan.FromMilliseconds(350)),
                EasingFunction = new CubicEase()
            };

            public static DoubleAnimation LibrarySwitchButton__1 = new DoubleAnimation
            {
                To = 1,
                Duration = new Duration(TimeSpan.FromMilliseconds(350)),
                EasingFunction = new CubicEase()
            };

            public static DoubleAnimation CreditsSwitchButton__0 = new DoubleAnimation
            {
                To = 0,
                Duration = new Duration(TimeSpan.FromMilliseconds(350)),
                EasingFunction = new CubicEase()
            };

            public static DoubleAnimation CreditsSwitchButton__1 = new DoubleAnimation
            {
                To = 1,
                Duration = new Duration(TimeSpan.FromMilliseconds(350)),
                EasingFunction = new CubicEase()
            };

            public static DoubleAnimation AddLibraryItemButton__0 = new DoubleAnimation
            {
                To = 0,
                Duration = new Duration(TimeSpan.FromMilliseconds(350)),
                EasingFunction = new CubicEase()
            };

            public static DoubleAnimation AddLibraryItemButton__1 = new DoubleAnimation
            {
                To = 1,
                Duration = new Duration(TimeSpan.FromMilliseconds(350)),
                EasingFunction = new CubicEase()
            };

            public static DoubleAnimation CreditsSectionGrid__1 = new DoubleAnimation
            {
                To = 1,
                Duration = new Duration(TimeSpan.FromMilliseconds(350)),
                EasingFunction = new CubicEase()
            };

            public static DoubleAnimation CreditsSectionGrid__0 = new DoubleAnimation
            {
                To = 0,
                Duration = new Duration(TimeSpan.FromMilliseconds(350)),
                EasingFunction = new CubicEase()
            };

            public static DoubleAnimation LibrarySectionGrid__1 = new DoubleAnimation
            {
                To = 1,
                Duration = new Duration(TimeSpan.FromMilliseconds(350)),
                EasingFunction = new CubicEase()
            };

            public static DoubleAnimation LibrarySectionGrid__0 = new DoubleAnimation
            {
                To = 0,
                Duration = new Duration(TimeSpan.FromMilliseconds(350)),
                EasingFunction = new CubicEase()
            };
        }

        private void MasterWindow_Loaded(object sender, RoutedEventArgs e)
        {
            PopulateLibraryWrapPanel();
        }

        private void TitleBarGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                this.DragMove();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState != WindowState.Maximized)
            {
                MaximizeButton.Content = MDL2FontIcons.RestoreIcon;
                this.WindowState = WindowState.Maximized;
            }
            else
            {
                MaximizeButton.Content = MDL2FontIcons.MaximizeIcon;
                this.WindowState = WindowState.Normal;
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private async void NavigationButton_Click(object sender, RoutedEventArgs e)
        {
            DoubleAnimation DoubleAnimation;
            ThicknessAnimation ThicknessAnimation;

            NavigationButton.IsEnabled = false;

            if (NavigationBarGrid.ActualWidth < 150)
            {
                DoubleAnimation = Animations.NavigationBar__150;
                DoubleAnimation.From = NavigationBarGrid.ActualWidth;
                NavigationBarGrid.BeginAnimation(WidthProperty, DoubleAnimation);

                ThicknessAnimation = Animations.ContentPresenter__149_40_0_0;
                ThicknessAnimation.From = ContentPresenterGrid.Margin;
                ContentPresenterGrid.BeginAnimation(MarginProperty, ThicknessAnimation);

                DoubleAnimation = Animations.NavigationButton__0;
                DoubleAnimation.From = NavigationButton.Foreground.Opacity;
                NavigationBarGrid.BeginAnimation(SolidColorBrush.OpacityProperty, DoubleAnimation);

                DoubleAnimation = Animations.LibrarySwitchButton__0;
                DoubleAnimation.From = LibrarySwitchButton.Foreground.Opacity;
                NavigationBarGrid.BeginAnimation(SolidColorBrush.OpacityProperty, DoubleAnimation);

                DoubleAnimation = Animations.CreditsSwitchButton__0;
                DoubleAnimation.From = SettingsSwitchButton.Foreground.Opacity;
                NavigationBarGrid.BeginAnimation(SolidColorBrush.OpacityProperty, DoubleAnimation);

                NavigationButton.Content = "Collapse";
                LibrarySwitchButton.Content = "Library";
                SettingsSwitchButton.Content = "Settings";

                NavigationButton.FontFamily = new FontFamily("Segoe UI Historic");
                LibrarySwitchButton.FontFamily = new FontFamily("Segoe UI Historic");
                SettingsSwitchButton.FontFamily = new FontFamily("Segoe UI Historic");

                DoubleAnimation = Animations.NavigationButton__1;
                DoubleAnimation.From = NavigationButton.Foreground.Opacity;
                NavigationBarGrid.BeginAnimation(SolidColorBrush.OpacityProperty, DoubleAnimation);

                DoubleAnimation = Animations.LibrarySwitchButton__1;
                DoubleAnimation.From = LibrarySwitchButton.Foreground.Opacity;
                NavigationBarGrid.BeginAnimation(SolidColorBrush.OpacityProperty, DoubleAnimation);

                DoubleAnimation = Animations.CreditsSwitchButton__1;
                DoubleAnimation.From = SettingsSwitchButton.Foreground.Opacity;
                NavigationBarGrid.BeginAnimation(SolidColorBrush.OpacityProperty, DoubleAnimation);
            }
            else
            {
                DoubleAnimation = Animations.NavigationBar__50;
                DoubleAnimation.From = NavigationBarGrid.ActualWidth;
                NavigationBarGrid.BeginAnimation(WidthProperty, DoubleAnimation);

                ThicknessAnimation = Animations.ContentPresenter__49_40_0_0;
                ThicknessAnimation.From = ContentPresenterGrid.Margin;
                ContentPresenterGrid.BeginAnimation(MarginProperty, ThicknessAnimation);

                DoubleAnimation = Animations.NavigationButton__0;
                DoubleAnimation.From = NavigationButton.Foreground.Opacity;
                NavigationBarGrid.BeginAnimation(SolidColorBrush.OpacityProperty, DoubleAnimation);

                DoubleAnimation = Animations.LibrarySwitchButton__0;
                DoubleAnimation.From = LibrarySwitchButton.Foreground.Opacity;
                NavigationBarGrid.BeginAnimation(SolidColorBrush.OpacityProperty, DoubleAnimation);

                DoubleAnimation = Animations.CreditsSwitchButton__0;
                DoubleAnimation.From = SettingsSwitchButton.Foreground.Opacity;
                NavigationBarGrid.BeginAnimation(SolidColorBrush.OpacityProperty, DoubleAnimation);

                NavigationButton.Content = MDL2FontIcons.NavigationIcon;
                LibrarySwitchButton.Content = MDL2FontIcons.LibraryIcon;
                SettingsSwitchButton.Content = MDL2FontIcons.GearIcon;

                NavigationButton.FontFamily = new FontFamily("Segoe MDL2 Assets");
                LibrarySwitchButton.FontFamily = new FontFamily("Segoe MDL2 Assets");
                SettingsSwitchButton.FontFamily = new FontFamily("Segoe MDL2 Assets");

                DoubleAnimation = Animations.NavigationButton__1;
                DoubleAnimation.From = NavigationButton.Foreground.Opacity;
                NavigationBarGrid.BeginAnimation(SolidColorBrush.OpacityProperty, DoubleAnimation);

                DoubleAnimation = Animations.LibrarySwitchButton__1;
                DoubleAnimation.From = LibrarySwitchButton.Foreground.Opacity;
                NavigationBarGrid.BeginAnimation(SolidColorBrush.OpacityProperty, DoubleAnimation);

                DoubleAnimation = Animations.CreditsSwitchButton__1;
                DoubleAnimation.From = SettingsSwitchButton.Foreground.Opacity;
                NavigationBarGrid.BeginAnimation(SolidColorBrush.OpacityProperty, DoubleAnimation);
            }

            await Task.Delay(350);
            NavigationButton.IsEnabled = true;
        }

        private async void SettingsSwitchButton_Click(object sender, RoutedEventArgs e)
        {
            DoubleAnimation DoubleAnimation;

            LibrarySwitchButton.IsEnabled = false;
            SettingsSwitchButton.IsEnabled = false;

            if (SettingsSectionGrid.Opacity < 1)
            {
                SettingsSectionGrid.Visibility = Visibility.Visible;

                DoubleAnimation = Animations.CreditsSectionGrid__1;
                DoubleAnimation.From = SettingsSectionGrid.Opacity;
                SettingsSectionGrid.BeginAnimation(OpacityProperty, DoubleAnimation);

                DoubleAnimation = Animations.LibrarySectionGrid__0;
                DoubleAnimation.From = SettingsSectionGrid.Opacity;
                LibrarySectionGrid.BeginAnimation(OpacityProperty, DoubleAnimation);

                await Task.Delay(350);
                LibrarySectionGrid.Visibility = Visibility.Hidden;
            }

            LibrarySwitchButton.IsEnabled = true;
            SettingsSwitchButton.IsEnabled = true;
        }

        private async void LibrarySwitchButton_Click(object sender, RoutedEventArgs e)
        {
            DoubleAnimation DoubleAnimation;

            LibrarySwitchButton.IsEnabled = false;
            SettingsSwitchButton.IsEnabled = false;

            if (LibrarySectionGrid.Opacity < 1)
            {
                LibrarySectionGrid.Visibility = Visibility.Visible;

                DoubleAnimation = Animations.CreditsSectionGrid__0;
                DoubleAnimation.From = SettingsSectionGrid.Opacity;
                SettingsSectionGrid.BeginAnimation(OpacityProperty, DoubleAnimation);

                DoubleAnimation = Animations.LibrarySectionGrid__1;
                DoubleAnimation.From = LibrarySectionGrid.Opacity;
                LibrarySectionGrid.BeginAnimation(OpacityProperty, DoubleAnimation);

                await Task.Delay(350);
                SettingsSectionGrid.Visibility = Visibility.Hidden;
            }

            LibrarySwitchButton.IsEnabled = true;
            SettingsSwitchButton.IsEnabled = true;
        }

        private void LibrarySearchTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            DoubleAnimation DoubleAnimation = new DoubleAnimation
            {
                From = LibrarySearchGrid.Width,
                To = LibrarySearchGrid.Width * 1.5,
                Duration = new Duration(TimeSpan.FromMilliseconds(500)),
                EasingFunction = new CubicEase()
            };

            LibrarySearchGrid.BeginAnimation(WidthProperty, DoubleAnimation);

            if (!string.IsNullOrEmpty(LibrarySearchTextBox.Text) && LibrarySearchTextBox.Text == "Search")
                LibrarySearchTextBox.Text = "";
        }

        private void LibrarySearchTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            DoubleAnimation DoubleAnimation = new DoubleAnimation
            {
                From = LibrarySearchGrid.Width,
                To = LibrarySearchGrid.Width / 1.5,
                Duration = new Duration(TimeSpan.FromMilliseconds(500)),
                EasingFunction = new CubicEase()
            };

            LibrarySearchGrid.BeginAnimation(WidthProperty, DoubleAnimation);

            if (string.IsNullOrEmpty(LibrarySearchTextBox.Text))
                LibrarySearchTextBox.Text = "Search";
        }

        private void LibrarySearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (LibrarySearchTextBox.Text != "Search" && !string.IsNullOrEmpty(LibrarySearchTextBox.Text))
                LibrarySearchArguments = LibrarySearchTextBox.Text.ToLower().Trim().Split(LibrarySearchArgumentSeperator, StringSplitOptions.RemoveEmptyEntries);
            else
                LibrarySearchArguments = [];

            if (LibraryWrapPanel != null)
                PopulateLibraryWrapPanel();
        }

        private void AddLibraryItemButton_Click(object sender, RoutedEventArgs e)
        {
            LibraryAddItemWindow DialogWindow = new LibraryAddItemWindow(this);
            DialogWindow.ShowDialog();
        }

        public void PopulateLibraryWrapPanel()
        {
            LibraryWrapPanel.Children.Clear();
            DirectoryInfo[] directories = new DirectoryInfo("./Library").GetDirectories();

            foreach (DirectoryInfo directoryInfo in directories)
            {
                string initPath = System.IO.Path.Combine(directoryInfo.FullName, "init.json");

                if (File.Exists(initPath))
                {
                    string initContent = File.ReadAllText(initPath);
                    InitLayout? init = JsonSerializer.Deserialize<InitLayout>(initContent);

                    if (init == null)
                    {
                        MessageBox.Show($"Unexpected System.Text.Json exception. Init was null.\nBroken registry for {directoryInfo.Name}, delete or repair registry.", "Argument Null Exception");
                        return;
                    }

                    if (init.IsValidRegistry != true) continue;

                    string[] nameArgs = init.Name.ToString().ToLower().Trim().Split(LibrarySearchArgumentSeperator, StringSplitOptions.RemoveEmptyEntries);
                    if (LibrarySearchArguments != null && LibrarySearchArguments.Length > 0)
                    {
                        bool containInSearch = false;

                        foreach (string nameArg in nameArgs)
                        {
                            foreach (string searchArg in LibrarySearchArguments)
                            {
                                if (searchArg != null && nameArg.Contains(searchArg))
                                {
                                    containInSearch = true;
                                    break;
                                }
                            }

                            if (containInSearch)
                                break;
                        }

                        if (!containInSearch)
                            continue;
                    }

                    ContextMenu contextMenu = new ContextMenu
                    {
                        Background = new BrushConverter().ConvertFromString("#FF1F1F1F") as SolidColorBrush,
                        Padding = new Thickness(0)
                    };

                    MenuItem DeleteItem = new MenuItem
                    {
                        Background = new BrushConverter().ConvertFromString("#FF222222") as SolidColorBrush,
                        Foreground = new BrushConverter().ConvertFromString("#FFCCCCCC") as SolidColorBrush,
                        Style = (Style)Application.Current.Resources["MenuItemBaseStyle"],
                        FontFamily = new FontFamily("Segoe UI Historic"),
                        Header = "Delete"
                    };

                    DeleteItem.Click += DeleteItem_Click;

                    contextMenu.Items.Add(DeleteItem);

                    Button LibraryItemMasterButton = new Button
                    {
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Top,
                        Margin = new Thickness(5, 10, 5, 10),
                        BorderThickness = new Thickness(0.5),
                        Background = new BrushConverter().ConvertFromString("#002C2C2C") as SolidColorBrush,
                        BorderBrush = new BrushConverter().ConvertFromString("#FF737373") as SolidColorBrush,
                        Style = (Style)Application.Current.Resources["FullTransparent-Opaque-Background-Button"],
                        Tag = directoryInfo.FullName
                    };

                    LibraryItemMasterButton.MouseEnter += LibraryItemMasterButton_MouseEnter;
                    LibraryItemMasterButton.MouseLeave += LibraryItemMasterButton_MouseLeave;
                    LibraryItemMasterButton.ContextMenu = contextMenu;

                    Grid LibraryItemGrid = new Grid
                    {
                        MaxWidth = 130,
                        MaxHeight = 200
                    };

                    Image LibraryItemCoverImage = new Image
                    {
                        Opacity = 1,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Top,
                        Stretch = Stretch.UniformToFill
                    };

                    if (!File.Exists(System.IO.Path.Combine(directoryInfo.FullName, "cover.png")))
                    {
                        LibraryItemCoverImage.Source = new BitmapImage(new Uri("pack://application:,,,/not-available.png"))
                        {
                            CacheOption = BitmapCacheOption.OnLoad
                        };

                        LibraryItemCoverImage.Tag = true;
                    }
                    else
                    {
                        LibraryItemCoverImage.Source = new BitmapImage(new Uri(System.IO.Path.Combine(directoryInfo.FullName, "cover.png")))
                        {
                            CacheOption = BitmapCacheOption.OnLoad
                        };

                        LibraryItemCoverImage.Tag = false;
                    }

                    TextBlock LibraryItemTextBlock = new TextBlock
                    {
                        MaxWidth = 98,
                        Margin = new Thickness(10, 0, 10, 50),
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        FontFamily = new FontFamily("Segoe UI Semibold"),
                        FontSize = 18,
                        Foreground = new BrushConverter().ConvertFromString("#FFCCCCCC") as SolidColorBrush,
                        Text = init.Name,
                        Opacity = 0,
                        TextAlignment = TextAlignment.Center,
                        TextWrapping = TextWrapping.Wrap

                    };

                    LibraryItemGrid.Children.Add(LibraryItemCoverImage);
                    LibraryItemGrid.Children.Add(LibraryItemTextBlock);
                    LibraryItemMasterButton.Content = LibraryItemGrid;

                    LibraryWrapPanel.Children.Add(LibraryItemMasterButton);
                }
                else MessageBox.Show($"Unexpected System.IO.File exception. Init file not found.\nBroken registry for {directoryInfo.Name}, delete or repair registry.", "File Not Found Exception");
            }
        }

        private void DeleteItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem? DeleteItem = sender as MenuItem;
            if (DeleteItem == null)
                return;

            ContextMenu? ContextMenu = DeleteItem.Parent as ContextMenu;
            if (ContextMenu == null)
                return;

            Button? LibraryItemMasterButton = ContextMenu.PlacementTarget as Button;
            if (LibraryItemMasterButton == null)
                return;

            WrapPanel? WrapPanel = LibraryItemMasterButton.Parent as WrapPanel;
            if (WrapPanel == null)
                return;

            WrapPanel.Children.Remove(LibraryItemMasterButton);

            if (LibraryItemMasterButton.Tag != null && Directory.Exists((string)LibraryItemMasterButton.Tag))
            {
                WrapPanel.Children.Remove(LibraryItemMasterButton);

                string InitPath = System.IO.Path.Combine((string)LibraryItemMasterButton.Tag, "init.json");
                if (!File.Exists(InitPath))
                    return;

                string InitContent = File.ReadAllText(InitPath);

                InitLayout? Init = JsonSerializer.Deserialize<InitLayout>(InitContent);
                if (Init == null) return;

                Init.IsValidRegistry = false;

                InitContent = JsonSerializer.Serialize<InitLayout>(Init);
                File.WriteAllText(InitPath, InitContent);
            }
        }

        private void LibraryItemMasterButton_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Button? LibraryItemMasterButton = sender as FrameworkElement as Button;
            if (LibraryItemMasterButton != null)
            {
                Grid? LibraryItemGrid = LibraryItemMasterButton.Content as Grid;
                if (LibraryItemGrid == null)
                    return;

                Image? LibraryItemCoverImage = LibraryItemGrid.Children[0] as Image;
                if (LibraryItemCoverImage == null)
                    return;

                TextBlock? LibraryItemTextBlock = LibraryItemGrid.Children[1] as TextBlock;

                if (LibraryItemGrid == null || LibraryItemCoverImage == null || LibraryItemTextBlock == null)
                    return;

                DoubleAnimation LibraryItemCoverImage_OpacityTo1 = new DoubleAnimation
                {
                    From = LibraryItemCoverImage.Opacity,
                    To = 1,
                    EasingFunction = new CubicEase(),
                    Duration = new Duration(TimeSpan.FromMilliseconds(350))
                };

                DoubleAnimation LibraryItemTextBlock_OpacityTo0 = new DoubleAnimation
                {
                    From = LibraryItemTextBlock.Opacity,
                    To = 0,
                    EasingFunction = new CubicEase(),
                    Duration = new Duration(TimeSpan.FromMilliseconds(350))
                };

                LibraryItemCoverImage.BeginAnimation(Rectangle.OpacityProperty, LibraryItemCoverImage_OpacityTo1);
                LibraryItemTextBlock.BeginAnimation(Rectangle.OpacityProperty, LibraryItemTextBlock_OpacityTo0);
            }
            else throw new NotImplementedException();
        }

        private void LibraryItemMasterButton_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Button? LibraryItemMasterButton = sender as FrameworkElement as Button;
            if (LibraryItemMasterButton != null)
            {
                Grid? LibraryItemGrid = LibraryItemMasterButton.Content as Grid;
                if (LibraryItemGrid == null)
                    return;

                Image? LibraryItemCoverImage = (LibraryItemGrid.Children[0] as Image);
                if (LibraryItemCoverImage == null)
                    return;

                TextBlock? LibraryItemTextBlock = (LibraryItemGrid.Children[1] as TextBlock);

                if (LibraryItemGrid == null || LibraryItemCoverImage == null || LibraryItemTextBlock == null)
                    return;

                DoubleAnimation LibraryItemCoverImage_OpacityTo03 = new DoubleAnimation
                {
                    From = LibraryItemCoverImage.Opacity,
                    To = 0.3,
                    EasingFunction = new CubicEase(),
                    Duration = new Duration(TimeSpan.FromMilliseconds(350))
                };

                if (LibraryItemCoverImage.Tag is bool)
                {
                    if ((bool)LibraryItemCoverImage.Tag == true)
                        LibraryItemCoverImage_OpacityTo03.To = 0;
                    else LibraryItemCoverImage_OpacityTo03.To = 0.3;
                }

                DoubleAnimation LibraryItemTextBlock_OpacityTo1 = new DoubleAnimation
                {
                    From = LibraryItemTextBlock.Opacity,
                    To = 1,
                    EasingFunction = new CubicEase(),
                    Duration = new Duration(TimeSpan.FromMilliseconds(350))
                };

                LibraryItemCoverImage.BeginAnimation(Rectangle.OpacityProperty, LibraryItemCoverImage_OpacityTo03);
                LibraryItemTextBlock.BeginAnimation(Rectangle.OpacityProperty, LibraryItemTextBlock_OpacityTo1);
            }
        }
    }
}