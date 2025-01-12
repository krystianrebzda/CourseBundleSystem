using CourseBundleSystem.Models;
using CourseBundleSystem.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace CourseBundleSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuoteController(IQuotesService quotesService) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<Dictionary<string, double>>> Post(TopicsRequest topicsRequest) 
        {
            var quotes = await quotesService.CalculateQuotes(topicsRequest);

            if (quotes.Count == 0)
            {
                return NotFound("No matching quotes were found for the provided topics.");
            }

            return Ok(quotes);
        }
    }
}
