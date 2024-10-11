using FluentAssertions;
using WpfPostApp.Services;

namespace WpfPostApp.Tests;

public class UtilitiesTests
{
    [Theory]
    [InlineData(1, 1, 1)]
    [InlineData(2, 2, 2)]
    [InlineData(3, 2, 2)]
    [InlineData(4, 2, 2)]
    [InlineData(5, 3, 3)]
    [InlineData(8, 3, 3)]
    [InlineData(9, 3, 3)]
    [InlineData(100, 10, 10)]
    public void CalculateGridSizes_ShouldReturnCorrectGridSizes(
        int numberOfItems,
        int expectedRows,
        int expectedCols
    )
    {
        // Act
        var (rows, cols) = Utilities.CalculateGridSizes(numberOfItems);

        // Assert
        rows.Should().Be(expectedRows);
        cols.Should().Be(expectedCols);
    }
}
