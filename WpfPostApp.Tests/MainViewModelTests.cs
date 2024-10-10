using System.Collections.ObjectModel;
using FluentAssertions;
using Moq;
using WpfPostApp.Models;
using WpfPostApp.Services;
using WpfPostApp.ViewModel;

namespace WpfPostApp.Tests;

public class MainViewModelTests
{
    private readonly Mock<IPostService> _mockPostService;
    private readonly MainViewModel _viewModel;

    public MainViewModelTests()
    {
        _mockPostService = new Mock<IPostService>();
        _viewModel = new MainViewModel(_mockPostService.Object);
    }

    [Fact]
    public void Constructor_Should_ThrowArgumentNullException_WhenPostServiceIsNull()
    {
        // Act
        Action act = () => new MainViewModel(null);

        // Assert
        act.Should().Throw<ArgumentNullException>().WithMessage("*postService*");
    }

    [Fact]
    public void Constructor_Should_InitializeProperties()
    {
        // Assert
        _viewModel.Posts.Should().BeEmpty();
        _viewModel.ShowUserId.Should().BeFalse();
        _viewModel.NRows.Should().Be(0);
        _viewModel.NCols.Should().Be(0);
    }

    [Fact]
    public void ChangeShownIdCommand_Should_ToggleShowUserId()
    {
        // Arrange
        var initialShowUserId = _viewModel.ShowUserId;

        // Act
        _viewModel.ChangeShownIdCommand.Execute(null);

        // Assert
        _viewModel.ShowUserId.Should().Be(!initialShowUserId);
    }

    [Fact]
    public async Task LoadPostsCommand_Should_UpdatePostsAndGridSize()
    {
        // Arrange
        var mockPosts = new ObservableCollection<Post>
        {
            new()
            {
                UserId = 1,
                Id = 1,
                Title = "Post 1",
                Body = "Content 1",
            },
            new()
            {
                UserId = 2,
                Id = 2,
                Title = "Post 2",
                Body = "Content 2",
            },
        };

        _mockPostService.Setup(service => service.GetPostsAsync()).ReturnsAsync(mockPosts);

        // Act
        await _viewModel.LoadPostsCommand.ExecuteAsync(null);

        // Assert
        _viewModel.Posts.Should().BeEquivalentTo(mockPosts);
        _viewModel.NRows.Should().BeGreaterThan(0);
        _viewModel.NCols.Should().BeGreaterThan(0);
        _mockPostService.Verify(service => service.GetPostsAsync(), Times.Once);
    }
}
