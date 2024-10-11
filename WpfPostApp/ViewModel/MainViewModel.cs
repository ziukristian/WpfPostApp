using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WpfPostApp.Models;
using WpfPostApp.Services;

namespace WpfPostApp.ViewModel;

public class MainViewModel : ObservableObject
{
    #region Fields

    private readonly IPostService _postService;

    // true : show userID | false : show postId
    private bool showUserId;
    public bool ShowUserId
    {
        get => showUserId;
        set => SetProperty(ref showUserId, value);
    }

    private ObservableCollection<Post> posts;
    public ObservableCollection<Post> Posts
    {
        get => posts;
        set => SetProperty(ref posts, value);
    }

    private int nCols;
    public int NCols
    {
        get => nCols;
        set => SetProperty(ref nCols, value);
    }

    private int nRows;
    public int NRows
    {
        get => nRows;
        set => SetProperty(ref nRows, value);
    }

    #endregion

    #region Commands
    public RelayCommand ChangeShownIdCommand { get; }
    public AsyncRelayCommand LoadPostsCommand { get; }

    #endregion

    public MainViewModel(IPostService postService)
    {
        _postService = postService ?? throw new ArgumentNullException(nameof(postService));
        ChangeShownIdCommand = new RelayCommand(ChangeShownId);
        LoadPostsCommand = new AsyncRelayCommand(LoadPosts);
        posts = [];
        showUserId = false;
    }

    #region Methods
    private void ChangeShownId()
    {
        ShowUserId = !ShowUserId;
    }

    private async Task LoadPosts()
    {
        var posts = await _postService.GetPostsAsync();

        (NRows, NCols) = Utilities.CalculateGridSizes(posts.Count);

        Posts = posts;
    }

    #endregion
}
