using CourseBundleSystem.Models;

namespace CourseBundleSystem.Services.Abstractions;

public interface IQuotesService
{
    Task<Dictionary<string, double>> CalculateQuotes(TopicsRequest topicsRequest);
}
