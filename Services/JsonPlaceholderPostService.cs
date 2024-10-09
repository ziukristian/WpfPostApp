using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;
using WpfPostApp.Models;

namespace WpfPostApp.Services;

internal class JsonPlaceholderPostService : IPostService
{
    private readonly HttpClient _httpClient =
        new() { BaseAddress = new Uri("https://jsonplaceholder.typicode.com") };

    public async Task<ObservableCollection<Post>> GetPostsAsync(int limit)
    {
        var posts = await _httpClient.GetFromJsonAsync<ObservableCollection<Post>>(
            $"posts?_limit={limit}"
        );
        return posts ?? [];
    }

    public async Task<ObservableCollection<Post>> GetPostsAsync()
    {
        var posts = await _httpClient.GetFromJsonAsync<ObservableCollection<Post>>("posts");
        return posts ?? [];
    }
}
