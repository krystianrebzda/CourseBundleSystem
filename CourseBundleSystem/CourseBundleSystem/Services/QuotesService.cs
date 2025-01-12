using CourseBundleSystem.Constants;
using CourseBundleSystem.Models;
using CourseBundleSystem.Services.Abstractions;
using Microsoft.Extensions.Options;

namespace CourseBundleSystem.Services;

public class QuotesService : IQuotesService
{
    private readonly ProviderTopicsConfig _providerTopicsConfig;

    public QuotesService(IOptions<ProviderTopicsConfig> providerTopicsConfig) => _providerTopicsConfig = providerTopicsConfig.Value;

    public async Task<Dictionary<string, double>> CalculateQuotes(TopicsRequest topicsRequest)
    {
        var providerTopics = _providerTopicsConfig.ProviderTopics;

        if (providerTopics == null) {
            throw new ArgumentNullException(nameof(providerTopics));
        }

        return await Task.Run(() => CalculateQuotesForRequest(topicsRequest, providerTopics));
    }

    private static Dictionary<string, double> CalculateQuotesForRequest(TopicsRequest topicsRequest, Dictionary<string, string> providerTopics)
    {
        var quotes = new Dictionary<string, double>();

        var topTopics = topicsRequest.Topics?.ToDictionary()
            .OrderByDescending(t => t.Value)
            .Take(ConstantValues.TopTopics)
            .ToList();

        foreach (var provider in providerTopics)
        {
            var providerName = provider.Key;
            var providerOfferedTopics = provider.Value.Split('+');

            var matches = topTopics?.Where(t => providerOfferedTopics.Contains(t.Key)).ToList();

            if (matches?.Count == 2)
            {
                double combinedLevel = matches.Sum(m => m.Value);
                quotes[providerName] = combinedLevel * 0.1;
            }

            if (matches?.Count == 1)
            {
                var match = matches.First();
                int index = topTopics.IndexOf(match);

                double percentage = index switch
                {
                    0 => 0.2,
                    1 => 0.25,
                    2 => 0.3,
                    _ => 0
                };

                quotes[providerName] = match.Value * percentage;
            }
        }

        return quotes.Where(q => q.Value > 0).ToDictionary(q => q.Key, q => q.Value);
    }
}
