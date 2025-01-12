using CourseBundleSystem.Models;
using CourseBundleSystem.Services;
using Microsoft.Extensions.Options;
using Moq;

namespace CourseBundleSystem.Tests.ServicesTests;

public class QuotesServiceTests
{
    private readonly QuotesService _quotesService;

    public QuotesServiceTests()
    {
        var mockProviderTopicsConfig = new Mock<IOptions<ProviderTopicsConfig>>();
        mockProviderTopicsConfig.Setup(config => config.Value).Returns(new ProviderTopicsConfig
        {
            ProviderTopics = new Dictionary<string, string>
            {
                { "provider_a", "math+science" },
                { "provider_b", "reading+science" },
                { "provider_c", "history+math" }
            }
        });

        _quotesService = new QuotesService(mockProviderTopicsConfig.Object);
    }

    [Fact]
    public async Task CalculateQuotes_ThrowsArgumentNullException_WhenProviderTopicsIsNull()
    {
        // Arrange
        var mockProviderTopicsConfig = new Mock<IOptions<ProviderTopicsConfig>>();
        mockProviderTopicsConfig.Setup(config => config.Value).Returns(new ProviderTopicsConfig
        {
            ProviderTopics = null
        });

        var quotesService = new QuotesService(mockProviderTopicsConfig.Object);

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

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => quotesService.CalculateQuotes(topicsRequest));
    }

    [Fact]
    public async Task CalculateQuotes_ReturnsCorrectQuotes_ForValidRequest()
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

        // Act
        var quotes = await _quotesService.CalculateQuotes(topicsRequest);

        // Assert
        Assert.NotNull(quotes);
        Assert.Equal(3, quotes.Count);
        Assert.Equal(8.0, quotes["provider_a"]);
        Assert.Equal(5.0, quotes["provider_b"]);
        Assert.Equal(10.0, quotes["provider_c"]);
    }

    [Fact]
    public async Task CalculateQuotes_ReturnsEmptyDictionary_WhenNoMatchesFound()
    {
        // Arrange
        var topicsRequest = new TopicsRequest
        {
            Topics = new Topics
            {
                Art = 10
            }
        };

        // Act
        var quotes = await _quotesService.CalculateQuotes(topicsRequest);

        // Assert
        Assert.NotNull(quotes);
        Assert.Empty(quotes);
    }
}
