using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WpfPostApp.Models;

namespace WpfPostApp.ViewModel;

public class MainViewModel : ObservableObject
{
    private readonly HttpClient _httpClient;

    private bool showUserId = false;
    public bool ShowUserId
    {
        get => showUserId;
        set => SetProperty(ref showUserId, value);
    }

    private ObservableCollection<Post>? posts = [];
    public ObservableCollection<Post>? Posts
    {
        get => posts;
        set => SetProperty(ref posts, value);
    }

    // test

    public MainViewModel()
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://jsonplaceholder.typicode.com"),
        };

        ChangeShownIdCommand = new RelayCommand(ChangeShownId);
        LoadPostsCommand = new AsyncRelayCommand(LoadPosts);
    }

    public RelayCommand ChangeShownIdCommand { get; }
    public AsyncRelayCommand LoadPostsCommand { get; }

    private void ChangeShownId()
    {
        ShowUserId = !ShowUserId;
    }

    private async Task LoadPosts()
    {
        var posts = await _httpClient.GetFromJsonAsync<ObservableCollection<Post>>(
            "posts?_limit=100"
        );

        if (posts == null)
            return;

        Posts = posts;

        //Posts?.Clear();
        //foreach (var post in posts)
        //{
        //    Posts.Add(post);
        //}
    }
}
