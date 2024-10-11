using System.Collections.ObjectModel;
using FluentAssertions;
using Moq;
using Moq.Protected;
using WpfPostApp.Models;
using WpfPostApp.Services;

namespace WpfPostApp.Tests;

public class JsonPlaceholderPostServiceTests
{
    private readonly Mock<HttpMessageHandler> _mockHandler;
    private readonly HttpClient _httpClient;
    private readonly JsonPlaceholderPostService _service;

    public JsonPlaceholderPostServiceTests()
    {
        _mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        _httpClient = new HttpClient(_mockHandler.Object)
        {
            BaseAddress = new Uri("https://jsonplaceholder.typicode.com"),
        };
        _service = new JsonPlaceholderPostService(_httpClient);
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenHttpClientIsNull()
    {
        // Act
        Action act = () => new JsonPlaceholderPostService(null);

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'httpClient')");
    }

    [Fact]
    public async Task GetPostsAsync_ShouldReturnPosts_WhenApiCallIsSuccessful()
    {
        // Arrange
        var expectedPosts = new ObservableCollection<Post>
        {
            new Post
            {
                Id = 1,
                UserId = 1,
                Title = "Test Post 1",
                Body = "Test Body 1",
            },
            new Post
            {
                Id = 2,
                UserId = 2,
                Title = "Test Post 2",
                Body = "Test Body 2",
            },
        };

        var httpResponse = new HttpResponseMessage
        {
            StatusCode = System.Net.HttpStatusCode.OK,
            Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(expectedPosts)),
        };

        _mockHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(httpResponse);

        // Act
        var result = await _service.GetPostsAsync();

        // Assert
        result.Should().BeEquivalentTo(expectedPosts);
    }

    [Fact]
    public async Task GetPostsAsync_ShouldReturnErrorPost_WhenApiCallFails()
    {
        // Arrange
        _mockHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ThrowsAsync(new HttpRequestException("Error occurred"));

        // Act
        var result = await _service.GetPostsAsync();

        // Assert
        result.Should().HaveCount(1);
        result[0].Title.Should().Be("ERROR: Loading failed");
        result[0].Id.Should().Be(0);
        result[0].UserId.Should().Be(0);
    }

    [Fact]
    public async Task GetPostsAsync_ShouldReturnEmptyCollection_WhenApiReturnsNull()
    {
        // Arrange
        var httpResponse = new HttpResponseMessage
        {
            StatusCode = System.Net.HttpStatusCode.OK,
            Content = new StringContent("null"),
        };

        _mockHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(httpResponse);

        // Act
        var result = await _service.GetPostsAsync();

        // Assert
        result.Should().BeEmpty();
    }
}
