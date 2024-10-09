using System.Windows;
using WpfPostApp.Services;
using WpfPostApp.ViewModel;

namespace WpfPostApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            // Inject post service
            var postService = new JsonPlaceholderPostService();

            var mainViewModel = new MainViewModel(postService);
            // Initial load for posts
            mainViewModel.LoadPostsCommand.ExecuteAsync(null);

            DataContext = mainViewModel;

            InitializeComponent();
        }

        private void Window_MouseLeftButtonDown(
            object sender,
            System.Windows.Input.MouseButtonEventArgs e
        )
        {
            DragMove();
        }

        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void BtnMaximize_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
            }
            else
            {
                WindowState = WindowState.Maximized;
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            // Not using Close() in case there's multiple windows one day
            Application.Current.Shutdown();
        }
    }
}
