using ResultBoxes;
using Sekiban.Core.Aggregate;
using Sekiban.Core.Query;
using Sekiban.Core.Query.QueryModel;
using SekibanPrj.Domain;

namespace SekibanPrj.Web;

public class WeatherApiClient(HttpClient httpClient)
{
    public async Task<WeatherForecast[]> GetWeatherAsync(int maxItems = 10, CancellationToken cancellationToken = default)
    {
        List<WeatherForecast>? forecasts = null;

        await foreach (var forecast in httpClient.GetFromJsonAsAsyncEnumerable<WeatherForecast>("/weatherforecast", cancellationToken))
        {
            if (forecasts?.Count >= maxItems)
            {
                break;
            }
            if (forecast is not null)
            {
                forecasts ??= [];
                forecasts.Add(forecast);
            }
        }

        return forecasts?.ToArray() ?? [];
    }
    public async Task<ListQueryResult<QueryAggregateState< BaseballTeam>>> GetBaseballTeamsAsync(CancellationToken cancellationToken = default)
    {
        return await httpClient.GetFromJsonAsync<ListQueryResult<QueryAggregateState< BaseballTeam>>>("/api/query/baseballteam/simpleaggregatelistquery1", cancellationToken) ?? ListQueryResult<QueryAggregateState< BaseballTeam>>.Empty;
    }
}

public record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
