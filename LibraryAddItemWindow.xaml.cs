using Microsoft.Win32;
using System.Globalization;
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
        private BitmapFunctions BitmapFunctions = new BitmapFunctions();

        public LibraryAddItemWindow(MainWindow _MainWindow_)
        {
            InitializeComponent();

            MainWindow = _MainWindow_;
            MasterWindow.Opacity = 0;
        }

        public class RegistryStructure
        {
            public required string DisplayName { get; set; }
            public string? Author { get; set; }
            public string? IMDB_Link { get; set; }
            public double? IMDB_Score { get; set; }
            public string? CoverBitmap { get; set; }
            public int? FilmLenght {  get; set; }
            public int? FilmResume {  get; set; }
            public int? SeriesSeason {  get; set; }
            public int? SeriesEpisode {  get; set; }
            public required bool IsValidRegistry { get; set; }
            public required bool RegistryType { get; set; }
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


            public static DoubleAnimation DoubleAnimation__0 = new DoubleAnimation
            {
                To = 0,
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

        private void DragDropButton_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                string[] validExtensions = { ".jpg", ".jpeg", ".png", ".bmp", ".gif" };
                string fileExtension = System.IO.Path.GetExtension(files[0]).ToLower();

                DoubleAnimation DoubleAnimation;

                if (Array.Exists(validExtensions, ext => ext == fileExtension))
                {
                    DoubleAnimation = Animations.DoubleAnimation__1;
                    DoubleAnimation.From = DragDropImage.Opacity;
                    DragDropImage.BeginAnimation(OpacityProperty, DoubleAnimation);

                    DoubleAnimation = Animations.DoubleAnimation__0_5;
                    DoubleAnimation.From = SelectedImage.Opacity;
                    SelectedImage.BeginAnimation(OpacityProperty, DoubleAnimation);
                }
                else
                {
                    DoubleAnimation = Animations.DoubleAnimation__0_5;
                    DoubleAnimation.From = DragDropImage.Opacity;
                    DragDropImage.BeginAnimation(OpacityProperty, DoubleAnimation);

                    DoubleAnimation = Animations.DoubleAnimation__1;
                    DoubleAnimation.From = UnavailableImage.Opacity;
                    UnavailableImage.BeginAnimation(OpacityProperty, DoubleAnimation);

                    DoubleAnimation = Animations.DoubleAnimation__0_5;
                    DoubleAnimation.From = SelectedImage.Opacity;
                    SelectedImage.BeginAnimation(OpacityProperty, DoubleAnimation);
                }
            }
        }

        private void DragDropButton_DragLeave(object sender, DragEventArgs e)
        {
            DoubleAnimation DoubleAnimation;

            if (SelectedImage.Source == null)
            {
                DoubleAnimation = Animations.DoubleAnimation__1;
                DoubleAnimation.From = DragDropImage.Opacity;
                DragDropImage.BeginAnimation(OpacityProperty, DoubleAnimation);
            }
            else
            {
                DoubleAnimation = Animations.DoubleAnimation__0;
                DoubleAnimation.From = DragDropImage.Opacity;
                DragDropImage.BeginAnimation(OpacityProperty, DoubleAnimation);
            }

            DoubleAnimation = Animations.DoubleAnimation__0;
            DoubleAnimation.From = UnavailableImage.Opacity;
            UnavailableImage.BeginAnimation(OpacityProperty, DoubleAnimation);

            DoubleAnimation = Animations.DoubleAnimation__1;
            DoubleAnimation.From = SelectedImage.Opacity;
            SelectedImage.BeginAnimation(OpacityProperty, DoubleAnimation);
        }

        private void DragDropButton_Drop(object sender, DragEventArgs e)
        {
            DoubleAnimation DoubleAnimation;

            DoubleAnimation = Animations.DoubleAnimation__0;
            DoubleAnimation.From = DragDropImage.Opacity;
            DragDropImage.BeginAnimation(OpacityProperty, DoubleAnimation);

            DoubleAnimation = Animations.DoubleAnimation__1;
            DoubleAnimation.From = SelectedImage.Opacity;
            SelectedImage.BeginAnimation(OpacityProperty, DoubleAnimation);

            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            string[] validExtensions = { ".jpg", ".jpeg", ".png", ".bmp", ".gif" };
            string fileExtension = System.IO.Path.GetExtension(files[0]).ToLower();

            if (files.Length > 0 && Array.Exists(validExtensions, ext => ext == fileExtension))
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(files[0], UriKind.Absolute);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();

                SelectedImage.Source = bitmap;
                SelectedImageDimensionLabel.Content = bitmap.PixelHeight.ToString() + 'x' + bitmap.PixelWidth.ToString();
            }
        }

        private void DragDropButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png, *.bmp, *.gif)|*.jpg;*.jpeg;*.png;*.bmp;*.gif";

            if (openFileDialog.ShowDialog() == true)
            {
                string selectedFilePath = openFileDialog.FileName;

                string[] validExtensions = { ".jpg", ".jpeg", ".png", ".bmp", ".gif" };
                string fileExtension = System.IO.Path.GetExtension(selectedFilePath).ToLower();

                if (Array.Exists(validExtensions, ext => ext == fileExtension))
                {
                    DoubleAnimation DoubleAnimation;

                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(selectedFilePath, UriKind.Absolute);
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();

                    SelectedImage.Source = bitmap;
                    SelectedImageDimensionLabel.Content = bitmap.PixelHeight.ToString() + 'x' + bitmap.PixelWidth.ToString();

                    DoubleAnimation = Animations.DoubleAnimation__0;
                    DoubleAnimation.From = DragDropImage.Opacity;
                    DragDropImage.BeginAnimation(OpacityProperty, DoubleAnimation);

                    DoubleAnimation = Animations.DoubleAnimation__1;
                    DoubleAnimation.From = SelectedImage.Opacity;
                    SelectedImage.BeginAnimation(OpacityProperty, DoubleAnimation);
                }
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e) => MasterWindow.Close();

        private void FinishButton_Click(object sender, RoutedEventArgs e)
        {
            if (NameTextBox.Text == "Enter Name" || string.IsNullOrEmpty(NameTextBox.Text))
            {
                MessageBox.Show("Property DisplayName cannot be null.", "NullArgumentException");
                return;
            }

            if (!double.TryParse(IMDBScoreTextBox.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double Score) || Score < 0 || Score > 10)
            {
                MessageBox.Show("Property IMDBScore has to be between 0 and 10." + Score, "ArgumentOutOfRangeException");
                return;
            }

            if (FilmButton.IsChecked == true)
            {
                if (string.IsNullOrEmpty(LenghtTextBox.Text))
                {
                    MessageBox.Show("Property Lenght cannot be null.", "NullArgumentException");
                    return;
                }

                if (string.IsNullOrEmpty(ResumeTextBox.Text))
                {
                    MessageBox.Show("Property Resume cannot be null.", "NullArgumentException");
                    return;
                }
            }

            

            RegistryStructure registryStructure = new()
            {
                DisplayName = NameTextBox.Text,
                Author = string.IsNullOrEmpty(AuthorTextBox.Text) ? "Unknown" : AuthorTextBox.Text,
                IMDB_Link = string.IsNullOrEmpty(IMDBLinkTextBox.Text) ? "Unknown" : IMDBLinkTextBox.Text,
                CoverBitmap = SelectedImage.Source != null ? BitmapFunctions.ImageSourceToBase64String(SelectedImage.Source) : null,
                IMDB_Score = Score,
                IsValidRegistry = true,
                RegistryType = FilmButton.IsChecked != null && FilmButton.IsChecked == true ? true : false
            };

            if (registryStructure.RegistryType == true)
            {
                registryStructure.FilmLenght = int.TryParse(LenghtTextBox.Text, out int Lenght) ? Lenght : 0;
                registryStructure.FilmResume = int.TryParse(ResumeTextBox.Text, out int Resume) ? Resume : 0;
            }
            else
            {
                registryStructure.SeriesSeason = int.TryParse(SeasonTextBox.Text, out int Season) ? Season : 0;
                registryStructure.SeriesEpisode = int.TryParse(EpisodeTextBox.Text, out int Episode) ? Episode : 1;
            }

            string RegistryPath = Path.Combine("./Library", NameTextBox.Text + ".json");

            string RegistryContent = JsonSerializer.Serialize<RegistryStructure>(registryStructure);
            File.WriteAllText(RegistryPath, RegistryContent);

            this.Close();
        }

        private void FilmButton_Checked(object sender, RoutedEventArgs e)
        {
            DoubleAnimation DoubleAnimation;

            if (SeriesGrid == null || FilmGrid == null)
                return;

            DoubleAnimation = Animations.DoubleAnimation__0;
            DoubleAnimation.From = SeriesGrid.Opacity;
            SeriesGrid.BeginAnimation(OpacityProperty, DoubleAnimation);

            FilmGrid.Visibility = Visibility.Visible;

            DoubleAnimation = Animations.DoubleAnimation__1;
            DoubleAnimation.From = FilmGrid.Opacity;
            FilmGrid.BeginAnimation(OpacityProperty, DoubleAnimation);

            SeriesGrid.Visibility = Visibility.Hidden;
            SeriesButton.IsChecked = false;
        }

        private void SeriesButton_Checked(object sender, RoutedEventArgs e)
        {
            DoubleAnimation DoubleAnimation;

            if (SeriesGrid == null || FilmGrid == null)
                return;

            DoubleAnimation = Animations.DoubleAnimation__0;
            DoubleAnimation.From = FilmGrid.Opacity;
            FilmGrid.BeginAnimation(OpacityProperty, DoubleAnimation);

            SeriesGrid.Visibility = Visibility.Visible;

            DoubleAnimation = Animations.DoubleAnimation__1;
            DoubleAnimation.From = SeriesGrid.Opacity;
            SeriesGrid.BeginAnimation(OpacityProperty, DoubleAnimation);

            FilmGrid.Visibility = Visibility.Hidden;
            FilmButton.IsChecked = false;
        }
    }
}