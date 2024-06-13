#pragma warning disable IDE0090 // Use 'new(...)'

using Microsoft.Win32;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace AniFlow_.NET
{
    public partial class LibraryAddItemWindow : Window
    {
        private MainWindow MainWindow;

        public LibraryAddItemWindow(MainWindow _MainWindow_)
        {
            InitializeComponent();
            MainWindow = _MainWindow_;
        }

        public class InitLayout
        {
            public required string Name { get; set; }
            public string[]? Aliases { get; set; }
            public string? Author { get; set; }
            public string? IMDB_Link { get; set; }
            public required bool IsValidRegistry { get; set; }
        }

        internal class Animations
        {
            public static DoubleAnimation DoubleAnimation__1 = new DoubleAnimation
            {
                To = 1,
                Duration = new Duration(TimeSpan.FromMilliseconds(350)),
                EasingFunction = new CubicEase()
            };

            public static DoubleAnimation DoubleAnimation__0_5 = new DoubleAnimation
            {
                To = 0.5,
                Duration = new Duration(TimeSpan.FromMilliseconds(350)),
                EasingFunction = new CubicEase()
            };
        }

        private void MasterWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DoubleAnimation DoubleAnimation;

            DoubleAnimation = Animations.DoubleAnimation__1;
            DoubleAnimation.From = MasterWindow.Opacity;
            MasterWindow.BeginAnimation(OpacityProperty, DoubleAnimation);
        }

        private void TitleBarGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                this.DragMove();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void UploadButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog OpenFileDialog = new OpenFileDialog
            {
                Filter = "Image Files (*.bmp;*.jpg;*.jpeg;*.png;*.gif;*.tiff)|*.bmp;*.jpg;*.jpeg;*.png;*.gif;*.tiff|All files (*.*)|*.*",
                InitialDirectory = AppDomain.CurrentDomain.BaseDirectory,
                Multiselect = false,
                Title = "Select File",
                CheckFileExists = true,
                CheckPathExists = true
            };

            if (OpenFileDialog.ShowDialog() == true)
            {
                string SelectedFilePath = OpenFileDialog.FileName;
                UploadedImage.Source = new BitmapImage(new Uri(SelectedFilePath))
                {
                    CacheOption = BitmapCacheOption.OnLoad
                };

                ThumbnailImage.Source = new BitmapImage(new Uri("pack://application:,,,/dashed-border.png"))
                {
                    CacheOption = BitmapCacheOption.OnLoad
                };
            }
        }

        private void UploadButton_DragEnter(object sender, DragEventArgs e)
        {
            DoubleAnimation DoubleAnimation;

            DoubleAnimation = Animations.DoubleAnimation__0_5;
            DoubleAnimation.From = UploadedImage.Opacity;
            UploadedImage.BeginAnimation(OpacityProperty, DoubleAnimation);
        }

        private void UploadButton_DragLeave(object sender, DragEventArgs e)
        {
            DoubleAnimation DoubleAnimation;

            DoubleAnimation = Animations.DoubleAnimation__1;
            DoubleAnimation.From = UploadedImage.Opacity;
            UploadedImage.BeginAnimation(OpacityProperty, DoubleAnimation);
        }

        private void UploadButton_Drop(object sender, DragEventArgs e)
        {
            DoubleAnimation DoubleAnimation;

            DoubleAnimation = Animations.DoubleAnimation__1;
            DoubleAnimation.From = UploadedImage.Opacity;
            UploadedImage.BeginAnimation(OpacityProperty, DoubleAnimation);

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] DroppedFiles = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (DroppedFiles.Length > 0)
                {
                    UploadedImage.Source = new BitmapImage(new Uri(DroppedFiles[0]))
                    {
                        CacheOption = BitmapCacheOption.OnLoad
                    };
                }
            }
        }

        private void ItemNameTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(ItemNameTextBox.Text) && ItemNameTextBox.Text == "Enter Anime Name")
                ItemNameTextBox.Text = "";
        }

        private void ItemNameTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(ItemNameTextBox.Text))
                ItemNameTextBox.Text = "Enter Anime Name";
        }

        private void ItemAuthorTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(ItemAuthorTextBox.Text) && ItemAuthorTextBox.Text == "Enter Anime Author")
                ItemAuthorTextBox.Text = "";
        }

        private void ItemAuthorTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(ItemAuthorTextBox.Text))
                ItemAuthorTextBox.Text = "Enter Anime Author";
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(ItemNameTextBox.Text) || ItemNameTextBox.Text == "Enter Anime Name")
            {
                MessageBox.Show("Item name can't be null or empty.\nEnter a name and try again.", "Argument Null Exception", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }

            string RegistryPath = System.IO.Path.Combine("./Library", Guid.NewGuid().ToString("D"));
            Directory.CreateDirectory(RegistryPath);

            string CoverPath = System.IO.Path.Combine(RegistryPath, "cover.png");
            if (UploadedImage.Source != null && UploadedImage.Source is BitmapSource)
            {
                BitmapSource BitmapSource = (BitmapSource)UploadedImage.Source;

                JpegBitmapEncoder Encoder = new JpegBitmapEncoder();
                Encoder.Frames.Add(BitmapFrame.Create(BitmapSource));

                using (FileStream fileStream = new FileStream(CoverPath, FileMode.Create))
                    Encoder.Save(fileStream);
            }

            string InitPath = System.IO.Path.Combine(RegistryPath, "init.json");
            string InitContent = JsonSerializer.Serialize<InitLayout>(new InitLayout
            {
                Name = ItemNameTextBox.Text,
                Aliases = null,
                Author = string.IsNullOrEmpty(ItemAuthorTextBox.Text) ? "Unknown" : ItemAuthorTextBox.Text,
                IMDB_Link = null,
                IsValidRegistry = true
            });

            File.WriteAllText(InitPath, InitContent);
            MainWindow.PopulateLibraryWrapPanel();

            this.Close();
        }
    }
}