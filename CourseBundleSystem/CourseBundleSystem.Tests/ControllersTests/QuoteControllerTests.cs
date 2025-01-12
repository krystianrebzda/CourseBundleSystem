using CourseBundleSystem.Controllers;
using CourseBundleSystem.Models;
using CourseBundleSystem.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CourseBundleSystem.Tests.ControllersTests;

public class QuoteControllerTests
{
    private readonly Mock<IQuotesService> _mockQuotesService;
    private readonly QuoteController _controller;

    public QuoteControllerTests()
    {
        _mockQuotesService = new Mock<IQuotesService>();
        _controller = new QuoteController(_mockQuotesService.Object);
    }

    [Fact]
    public async Task Post_ReturnsNotFound_WhenNoQuotesAreFound()
    {
        // Arrange
        var topicsRequest = new TopicsRequest
        {
            Topics = new Topics
            {
                Reading = 20,
                Math = 50,
                Science = 30,
                History = 15,
                Art = 10
            }
        };

        _mockQuotesService
            .Setup(service => service.CalculateQuotes(topicsRequest))
            .ReturnsAsync(new Dictionary<string, double>());

        // Act
        var result = await _controller.Post(topicsRequest);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Equal("No matching quotes were found for the provided topics.", notFoundResult.Value);
    }

    [Fact]
    public async Task Post_ReturnsOk_WhenQuotesAreFound()
    {
        // Arrange
        var topicsRequest = new TopicsRequest
        {
            Topics = new Topics
            {
                Reading = 20,
                Math = 50,
                Science = 30,
                History = 15,
                Art = 10
            }
        };

        var quotes = new Dictionary<string, double>
        {
            { "provider_a", 8.0 },
            { "provider_b", 5.0 }
        };

        _mockQuotesService
            .Setup(service => service.CalculateQuotes(topicsRequest))
            .ReturnsAsync(quotes);

        // Act
        var result = await _controller.Post(topicsRequest);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedQuotes = Assert.IsType<Dictionary<string, double>>(okResult.Value);
        Assert.Equal(quotes, returnedQuotes);
    }
}
