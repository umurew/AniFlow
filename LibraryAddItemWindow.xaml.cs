#pragma warning disable IDE0090 // Use 'new(...)'

using Microsoft.Win32;
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
            MasterWindow.Opacity = 0;
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
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(selectedFilePath, UriKind.Absolute);
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();

                    SelectedImage.Source = bitmap;
                    SelectedImageDimensionLabel.Content = bitmap.PixelHeight.ToString() + 'x' + bitmap.PixelWidth.ToString();

                    DoubleAnimation DoubleAnimation;

                    DoubleAnimation = Animations.DoubleAnimation__0;
                    DoubleAnimation.From = DragDropImage.Opacity;
                    DragDropImage.BeginAnimation(OpacityProperty, DoubleAnimation);

                    DoubleAnimation = Animations.DoubleAnimation__1;
                    DoubleAnimation.From = SelectedImage.Opacity;
                    SelectedImage.BeginAnimation(OpacityProperty, DoubleAnimation);
                }
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
        MasterWindow.Close();
        }

        private void FinishButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(NameTextBox.Text) || NameTextBox.Text == "Enter Name")
            {
                MessageBox.Show("Registry name can't be null or empty.\nEnter a name and try again.", "Argument Null Exception", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }

            if (string.IsNullOrEmpty(SeasonTextBox.Text))
            {
                MessageBox.Show("Registry season can't be null or empty.\nEnter a name and try again.", "Argument Null Exception", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }
        }
    }
}