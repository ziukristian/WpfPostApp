using System.Collections.ObjectModel;
using WpfPostApp.Models;

namespace WpfPostApp.Services;

public interface IPostService
{
    public Task<ObservableCollection<Post>> GetPostsAsync();
}
