using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;
using WpfPostApp.Models;

namespace WpfPostApp.Services;

public class JsonPlaceholderPostService : IPostService
{
    private readonly HttpClient _httpClient;

    public JsonPlaceholderPostService(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public async Task<ObservableCollection<Post>> GetPostsAsync()
    {
        try
        {
            var posts = await _httpClient.GetFromJsonAsync<ObservableCollection<Post>>("posts");

            return posts ?? [];
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"An error occurred: {e.Message}");
            // It's just so I don't have to implement alerts for users
            return
            [
                new Post
                {
                    Id = 0,
                    UserId = 0,
                    Title = "ERROR: Loading failed",
                },
            ];
        }
    }
}
