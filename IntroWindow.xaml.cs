using System.Windows;
using System.Windows.Media.Animation;

namespace AniFlow_.NET
{
    public partial class IntroWindow : Window
    {
        public IntroWindow()
        {
            InitializeComponent();
        }

        private async void MasterWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ThicknessAnimation ThicknessAnimation = new ThicknessAnimation
            {
                From = SplashImage.Margin,
                To = new Thickness(0, 0, 0, 175),
                Duration = new Duration(TimeSpan.FromMilliseconds(1000)),
                EasingFunction = new CubicEase()
            };

            DoubleAnimation DoubleAnimation = new DoubleAnimation
            {
                From = SplashImage.Opacity,
                To = 1,
                Duration = new Duration(TimeSpan.FromMilliseconds(350)),
                EasingFunction = new CubicEase()
            };

            SplashImage.BeginAnimation(MarginProperty, ThicknessAnimation);
            SplashImage.BeginAnimation(OpacityProperty, DoubleAnimation);

            await Task.Delay(1200);

            DoubleAnimation = new DoubleAnimation
            {
                From = SplashImage.Opacity,
                To = 0,
                Duration = new Duration(TimeSpan.FromMilliseconds(350)),
                EasingFunction = new CubicEase()
            };
            SplashImage.BeginAnimation(OpacityProperty, DoubleAnimation);

            await Task.Delay(350);


            MainWindow MainWindow = new MainWindow();
            MainWindow.Show();

            this.Close();
        }
    }
}