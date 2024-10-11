using System.Net.Http;
using System.Windows;
using WpfPostApp.Services;
using WpfPostApp.ViewModel;

namespace WpfPostApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Generate client for postService
            HttpClient client =
                new() { BaseAddress = new Uri("https://jsonplaceholder.typicode.com") };
            // Inject post service
            var postService = new JsonPlaceholderPostService(client);

            var mainViewModel = new MainViewModel(postService);
            // Initial load for posts
            mainViewModel.LoadPostsCommand.ExecuteAsync(null);

            DataContext = mainViewModel;
        }
    }
}
