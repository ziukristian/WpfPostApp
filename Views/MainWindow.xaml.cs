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

            var mainViewModel = new MainViewModel(postService) { NumberOfPosts = 100 };
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

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnMaximize_Click(object sender, RoutedEventArgs e)
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

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            // Not using Close() in case there's multiple windows one day
            Application.Current.Shutdown();
        }
    }
}
