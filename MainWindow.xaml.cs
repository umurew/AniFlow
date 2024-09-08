#pragma warning disable IDE0090 // Use 'new(...)'

using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

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

        public class RegistryStructure
        {
            public required string DisplayName { get; set; }
            public string? Author { get; set; }
            public string? IMDB_Link { get; set; }
            public double? IMDB_Score { get; set; }
            public string? CoverBitmap { get; set; }
            public int? FilmLenght { get; set; }
            public int? FilmResume { get; set; }
            public int? SeriesSeason { get; set; }
            public int? SeriesEpisode { get; set; }
            public required bool RegistryType { get; set; }
        }

        internal class MDL2FontIcons
        {
            public static string RestoreIcon = "\xE923";
            public static string MaximizeIcon = "\xE922";
            public static string NavigationIcon = "\xE700";
            public static string LibraryIcon = "\xE1D3";
            public static string GearIcon = "\xE713";
            public static string AddIcon = "\xE710";
            public static string RefreshIcon = "\xE72C";
        }

        internal class Animations
        {
            public static DoubleAnimation DoubleAnimation__0 = new DoubleAnimation
            {
                To = 0,
                Duration = new Duration(TimeSpan.FromMilliseconds(150)),
                EasingFunction = new CubicEase()
            };

            public static DoubleAnimation DoubleAnimation__1 = new DoubleAnimation
            {
                To = 1,
                Duration = new Duration(TimeSpan.FromMilliseconds(150)),
                EasingFunction = new CubicEase()
            };
        }

        public void PopulateLibraryWrapPanel()
        {
            LibraryWrapPanel.Children.Clear();
            FileInfo[] Registeries = new DirectoryInfo("./Library").GetFiles();

            foreach (FileInfo Registry in Registeries)
            {
                RegistryStructure? DeserializedRegistry = null;

                try
                {
                    DeserializedRegistry = JsonSerializer.Deserialize<RegistryStructure>(File.ReadAllText(Registry.FullName));
                }
                catch (Exception Exception)
                {
                    Registry.Delete();
                    continue;
                }

                if (DeserializedRegistry == null)
                {
                    MessageBox.Show($"Unexpected System.Text.Json exception. Init was null.\nCorrupted registry for {Registry.Name}, delete or repair registry.", "ArgumentNullException");
                    return;
                }

                string[] NameArguments = DeserializedRegistry.DisplayName.ToString().ToLower().Trim().Split(LibrarySearchArgumentSeperator, StringSplitOptions.RemoveEmptyEntries);
                if (LibrarySearchArguments != null && LibrarySearchArguments.Length > 0)
                {
                    bool ValidSearchArguments = false;

                    foreach (string NameArgument in NameArguments)
                    {
                        foreach (string SearchArgument in LibrarySearchArguments)
                        {
                            if (SearchArgument != null && NameArgument.Contains(SearchArgument)) ValidSearchArguments = true; break;
                        }

                        if (ValidSearchArguments) break;
                    }

                    if (!ValidSearchArguments) continue;
                }

                ContextMenu ContextMenu = new ContextMenu
                {
                    Background = new BrushConverter().ConvertFromString("#FF1F1F1F") as SolidColorBrush,
                    Padding = new Thickness(0)
                };

                MenuItem DeleteMenuItem = new MenuItem
                {
                    Background = new BrushConverter().ConvertFromString("#FF222222") as SolidColorBrush,
                    Foreground = new BrushConverter().ConvertFromString("#FFCCCCCC") as SolidColorBrush,
                    Style = (Style)Application.Current.Resources["MenuItemBaseStyle"],
                    FontFamily = new FontFamily("Segoe UI Historic"),
                    Header = "Delete"
                };

                DeleteMenuItem.Click += DeleteItem_Click;
                ContextMenu.Items.Add(DeleteMenuItem);

                Button LibraryItemRootButton = new Button
                {
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(5, 10, 5, 10),
                    BorderThickness = new Thickness(0.5),
                    Background = new BrushConverter().ConvertFromString("#002C2C2C") as SolidColorBrush,
                    BorderBrush = new BrushConverter().ConvertFromString("#FF737373") as SolidColorBrush,
                    Style = (Style)Application.Current.Resources["FullTransparent-Opaque-Background-Button"],
                    Tag = Registry.FullName
                };

                LibraryItemRootButton.MouseEnter += LibraryItemMasterButton_MouseEnter;
                LibraryItemRootButton.MouseLeave += LibraryItemMasterButton_MouseLeave;
                LibraryItemRootButton.Click += LibraryItemRootButton_Click;
                LibraryItemRootButton.ContextMenu = ContextMenu;

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

                if (DeserializedRegistry.CoverBitmap == null)
                {
                    LibraryItemCoverImage.Source = new BitmapImage(new Uri("pack://application:,,,/not-available.png")) { CacheOption = BitmapCacheOption.OnLoad };
                    LibraryItemCoverImage.Tag = true;
                }
                else
                {
                    LibraryItemCoverImage.Source = BitmapFunctions.Base64ToImageSource(DeserializedRegistry.CoverBitmap);
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
                    Text = DeserializedRegistry.DisplayName,
                    Opacity = 0,
                    TextAlignment = TextAlignment.Center,
                    TextWrapping = TextWrapping.Wrap
                };

                LibraryItemGrid.Children.Add(LibraryItemCoverImage);
                LibraryItemGrid.Children.Add(LibraryItemTextBlock);
                LibraryItemRootButton.Content = LibraryItemGrid;

                LibraryWrapPanel.Children.Add(LibraryItemRootButton);
            }
        }

        private async void LibraryItemRootButton_Click(object sender, RoutedEventArgs e)
        {
            DoubleAnimation DoubleAnimation;

            string Registry = File.ReadAllText((sender as Button).Tag.ToString());
            RegistryStructure SerializedRegistry = JsonSerializer.Deserialize<RegistryStructure>(Registry);

            NameTextBox.Text = SerializedRegistry.DisplayName;
            AuthorTextBox.Text = SerializedRegistry.Author;
            IMDBScoreTextBox.Text = SerializedRegistry.IMDB_Score.ToString();

            IMDBLinkTextBox.Text = SerializedRegistry.DisplayName + " IMDB Page";

            if (SerializedRegistry.IMDB_Link == null)
            {
                IMDBLinkTextBox.Visibility = Visibility.Hidden;
                IMDBLinkTextBox.Tag = null;
            }
            else
            {
                IMDBLinkTextBox.Visibility = Visibility.Visible;
                IMDBLinkTextBox.Tag = SerializedRegistry.IMDB_Link;
            }

            if (SerializedRegistry.RegistryType)
            {
                FilmLenghtTextBox.Text = SerializedRegistry.FilmLenght.ToString();
                ResumeTextBox.Text = SerializedRegistry.FilmResume.ToString();

                FilmDetailGrid.Visibility = Visibility.Visible;
                SeriesDetailGrid.Visibility = Visibility.Hidden;
            }
            else
            {
                SeasonTextBox.Text = SerializedRegistry.SeriesSeason.ToString();
                EpisodeTextBox.Text = SerializedRegistry.SeriesEpisode.ToString();

                FilmDetailGrid.Visibility = Visibility.Hidden;
                SeriesDetailGrid.Visibility = Visibility.Visible;
            }

            if (SerializedRegistry.CoverBitmap != null)
            {
                CoverImage.Source = BitmapFunctions.Base64ToImageSource(SerializedRegistry.CoverBitmap);
                CoverImageBorder.BorderThickness = new Thickness(0);
            }
            else
            {
                CoverImage.Source = new BitmapImage(new Uri("pack://application:,,,/not-available.png")) { CacheOption = BitmapCacheOption.OnLoad };
                CoverImageBorder.BorderThickness = new Thickness(1);
            }

            DoubleAnimation = Animations.DoubleAnimation__0;
            DoubleAnimation.From = LibraryScrollViewer.Opacity;
            LibraryScrollViewer.BeginAnimation(OpacityProperty, DoubleAnimation);

            DoubleAnimation = Animations.DoubleAnimation__0;
            DoubleAnimation.From = SectionTitleGrid.Opacity;
            SectionTitleGrid.BeginAnimation(OpacityProperty, DoubleAnimation);

            await Task.Delay(350);

            DetailsGrid.Visibility = Visibility.Visible;
            LibraryScrollViewer.Visibility = Visibility.Hidden;
            SectionTitleGrid.Visibility = Visibility.Hidden;

            DoubleAnimation = Animations.DoubleAnimation__1;
            DoubleAnimation.From = DetailsGrid.Opacity;
            DetailsGrid.BeginAnimation(OpacityProperty, DoubleAnimation);
        }

        private void MasterWindow_Loaded(object sender, RoutedEventArgs e) => PopulateLibraryWrapPanel();

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

        private void ExitButton_Click(object sender, RoutedEventArgs e) => Environment.Exit(0);

        private async void SettingsSwitchButton_Click(object sender, RoutedEventArgs e)
        {
            DoubleAnimation DoubleAnimation;

            LibrarySwitchButton.IsEnabled = false;
            SettingsSwitchButton.IsEnabled = false;

            if (SettingsSectionGrid.Opacity < 1)
            {
                SettingsSectionGrid.Visibility = Visibility.Visible;

                DoubleAnimation = Animations.DoubleAnimation__1;
                DoubleAnimation.From = SettingsSectionGrid.Opacity;
                SettingsSectionGrid.BeginAnimation(OpacityProperty, DoubleAnimation);

                DoubleAnimation = Animations.DoubleAnimation__0;
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

                DoubleAnimation = Animations.DoubleAnimation__0;
                DoubleAnimation.From = SettingsSectionGrid.Opacity;
                SettingsSectionGrid.BeginAnimation(OpacityProperty, DoubleAnimation);

                DoubleAnimation = Animations.DoubleAnimation__1;
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
            if (Application.Current.Windows.Count > 2) return;

            LibraryAddItemWindow DialogWindow = new LibraryAddItemWindow(this);
            DialogWindow.Show();
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e) => PopulateLibraryWrapPanel();

        private void DeleteItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem? DeleteItem = sender as MenuItem;
            if (DeleteItem == null) return;

            ContextMenu? ContextMenu = DeleteItem.Parent as ContextMenu;
            if (ContextMenu == null) return;

            Button? LibraryItemRootButton = ContextMenu.PlacementTarget as Button;
            if (LibraryItemRootButton == null) return;

            if (LibraryItemRootButton.Tag != null)
            {
                if (!File.Exists(LibraryItemRootButton.Tag.ToString())) return;
                File.Delete(LibraryItemRootButton.Tag.ToString());

                PopulateLibraryWrapPanel();
            }
        }

        private void LibraryItemMasterButton_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Button? LibraryItemRootButton = sender as FrameworkElement as Button;
            if (LibraryItemRootButton != null)
            {
                Grid? LibraryItemGrid = LibraryItemRootButton.Content as Grid;
                if (LibraryItemGrid == null) return;

                Image? LibraryItemCoverImage = LibraryItemGrid.Children[0] as Image;
                if (LibraryItemCoverImage == null) return;

                TextBlock? LibraryItemTextBlock = LibraryItemGrid.Children[1] as TextBlock;

                if (LibraryItemGrid == null || LibraryItemCoverImage == null || LibraryItemTextBlock == null) return;

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

                LibraryItemCoverImage.BeginAnimation(System.Windows.Shapes.Rectangle.OpacityProperty, LibraryItemCoverImage_OpacityTo1);
                LibraryItemTextBlock.BeginAnimation(System.Windows.Shapes.Rectangle.OpacityProperty, LibraryItemTextBlock_OpacityTo0);
            }
            else throw new NotImplementedException();
        }

        private void LibraryItemMasterButton_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Button? LibraryItemRootButton = sender as FrameworkElement as Button;
            if (LibraryItemRootButton != null)
            {
                Grid? LibraryItemGrid = LibraryItemRootButton.Content as Grid;
                if (LibraryItemGrid == null) return;

                Image? LibraryItemCoverImage = (LibraryItemGrid.Children[0] as Image);
                if (LibraryItemCoverImage == null) return;

                TextBlock? LibraryItemTextBlock = (LibraryItemGrid.Children[1] as TextBlock);

                if (LibraryItemGrid == null || LibraryItemCoverImage == null || LibraryItemTextBlock == null) return;

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

                LibraryItemCoverImage.BeginAnimation(System.Windows.Shapes.Rectangle.OpacityProperty, LibraryItemCoverImage_OpacityTo03);
                LibraryItemTextBlock.BeginAnimation(System.Windows.Shapes.Rectangle.OpacityProperty, LibraryItemTextBlock_OpacityTo1);
            }
        }

        private void IMDBLinkTextBox_MouseEnter(object sender, MouseEventArgs e)
        {
            IMDBLinkTextBox.TextDecorations = TextDecorations.Underline;
            IMDBLinkTextBox.Foreground = new BrushConverter().ConvertFrom("#FF0078D7") as SolidColorBrush;
        }

        private void IMDBLinkTextBox_MouseLeave(object sender, MouseEventArgs e)
        {
            IMDBLinkTextBox.TextDecorations = null;
            IMDBLinkTextBox.Foreground = new BrushConverter().ConvertFrom("#FFFEC60F") as SolidColorBrush;
        }

        private void IMDBLinkTextBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (IMDBLinkTextBox.Tag != null)
                Process.Start(new ProcessStartInfo(IMDBLinkTextBox.Tag.ToString()));
        }

        private async void BackButton_Click(object sender, RoutedEventArgs e)
        {
            DoubleAnimation DoubleAnimation;

            DoubleAnimation = Animations.DoubleAnimation__0;
            DoubleAnimation.From = DetailsGrid.Opacity;
            DetailsGrid.BeginAnimation(OpacityProperty, DoubleAnimation);

            await Task.Delay(350);

            DetailsGrid.Visibility = Visibility.Hidden;
            LibraryScrollViewer.Visibility = Visibility.Visible;
            SectionTitleGrid.Visibility = Visibility.Visible;

            DoubleAnimation = Animations.DoubleAnimation__1;
            DoubleAnimation.From = LibraryScrollViewer.Opacity;
            LibraryScrollViewer.BeginAnimation(OpacityProperty, DoubleAnimation);

            DoubleAnimation = Animations.DoubleAnimation__1;
            DoubleAnimation.From = SectionTitleGrid.Opacity;
            SectionTitleGrid.BeginAnimation(OpacityProperty, DoubleAnimation);
        }
    }
}