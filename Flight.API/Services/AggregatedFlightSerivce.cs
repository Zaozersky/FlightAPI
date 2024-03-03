using System.Text;
using AutoMapper;
using FlightAPI.Common;
using FlightAPI.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace FlightAPI.Services
{
    public class AggregatedFlightService : IAggregatedFlightService
    {
        private readonly IMapper _mapper;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMemoryCache _cache;
        private readonly ILogger<AggregatedFlightService> _logger;
        private readonly AppSettings _settings;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="memoryCache">memory cache</param>
        /// <param name="logger">logger</param>
        /// <param name="options">application settings</param>
        public AggregatedFlightService(IHttpClientFactory httpClientFactory, IMemoryCache memoryCache, IMapper mapper,
            ILogger<AggregatedFlightService> logger, IOptions<AppSettings> options)
        {
            _httpClientFactory = httpClientFactory;
            _mapper = mapper;
            _cache = memoryCache;
            _settings = options.Value;
            _logger = logger;
        }

        /// <summary>
        /// Receive flights 
        /// </summary>
        /// <param name="query">query parameters (date, price, airline, etc)</param>
        /// <param name="cancellationToken">cancellation token</param>
        /// <returns>Flight data</returns>
        public async Task<IEnumerable<FlightDto>> GetFlights(QueryParameters query, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Trying to get flights with query: {query}");

            var tcs = new TaskCompletionSource<IEnumerable<FlightDto>>();

            bool isNeedToCached = false;
            var cacheKey = query.ToString();

            var cacheFlights = GetFlightsFromCache(cacheKey, ref isNeedToCached);

            if (cacheFlights != null)
            {
                tcs.SetResult(cacheFlights);
                return await tcs.Task;
            }

            var flights = await LoadFlightsFromExternAPI(cancellationToken);

            FilterFlights(query.Filter, ref flights);

            SortFlights(query.Order, ref flights);

            if (isNeedToCached)
            {
                SetFlightDataToCache(cacheKey, flights);
            }

            tcs.TrySetResult(flights);
            return await tcs.Task;
        }

        public async Task BookFlight(int flightId, string nameAPI, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Trying to book the flight: {flightId}");

            using var client = _httpClientFactory.CreateClient(nameAPI);
            var content = new StringContent(flightId.ToString(), Encoding.UTF8, "application/json");
            var response = await client.PatchAsync(client.BaseAddress, content, cancellationToken);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                throw new Exception(response.Content.ReadAsStringAsync().GetAwaiter().GetResult());
        }

        private async Task<IEnumerable<FlightDto>> LoadFlightsFromExternAPI(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Trying to load flights from extern data sources.");

            var dataSources = _settings.DataSources;

            var dataSourceAPINames = new List<string>();
            var addressToNameAPI = new Dictionary<string, string>();

            foreach (string ds in dataSources.Split(";", StringSplitOptions.RemoveEmptyEntries))
            {
                var pairNameWithAddress = ds.Split(": ", StringSplitOptions.RemoveEmptyEntries);

                var nameAPI = pairNameWithAddress[0].Trim();
                var address = pairNameWithAddress[1].Trim();
                dataSourceAPINames.Add(nameAPI);

                addressToNameAPI.Add(address, nameAPI);
            }

            var tasks = Enumerable.Range(0, dataSourceAPINames.Count).Select(i =>
            {
                var client = _httpClientFactory.CreateClient(dataSourceAPINames[i]);
                return client.GetAsync(client.BaseAddress, cancellationToken);
            });

            var responses = await Task.WhenAll(tasks);

            return await GetFlightList(responses, addressToNameAPI);
        }

        private async Task<IEnumerable<FlightDto>> GetFlightList(HttpResponseMessage[] responses, IDictionary<string, string> addressToNameAPI)
        {
            var flightList = new List<FlightDto>();

            foreach (var response in responses)
            {
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    throw new Exception(response.Content.ReadAsStringAsync().GetAwaiter().GetResult());

                var nameAPI = addressToNameAPI[response.RequestMessage.RequestUri.ToString()];
                var jsonData = await response.Content.ReadAsStringAsync();

                var jsonFlights = JsonConvert.DeserializeObject<IEnumerable<JsonFlight>>(jsonData);
                var flights = _mapper.Map<IEnumerable<FlightDto>>(jsonFlights);

                foreach (var item in flights)
                {
                    item.NameAPI = nameAPI;
                }

                flightList.AddRange(flights);
            }

            return flightList;
        }

        #region Filtering flights

        private void FilterFlights(string searchString, ref IEnumerable<FlightDto> flights)
        {
            _logger.LogInformation($"Trying to filter flights.");

            if (!string.IsNullOrEmpty(searchString))
            {
                var filters = searchString.Split("&", StringSplitOptions.RemoveEmptyEntries);

                foreach (var filter in filters)
                {
                    var nameValuePair = filter.Split("=", StringSplitOptions.RemoveEmptyEntries);

                    if (!nameValuePair.Any() || nameValuePair.Count() != 2)
                    {
                        throw new ArgumentException($"Bad filter's format in the query: {searchString}");
                    }

                    var field = nameValuePair[0].Trim();
                    var value = nameValuePair[1].Trim();

                    switch (field)
                    {
                        case "origin":
                            flights = flights.Where(s => !string.IsNullOrEmpty(s.Origin) && s.Origin.Contains(value));
                            break;
                        case "destination":
                            flights = flights.Where(s => !string.IsNullOrEmpty(s.Destination) && s.Destination.Contains(value));
                            break;
                        case "airline":
                            flights = flights.Where(s => !string.IsNullOrEmpty(s.Airline) && s.Airline.Contains(value));
                            break;
                        case "departure_date":
                            if (DateTime.TryParse(value, out DateTime departureDate))
                            {
                                flights = flights.Where(s => s.DepartureDate == departureDate);
                            }
                            break;
                        case "arrival_date":
                            if (DateTime.TryParse(value, out DateTime arrivalDate))
                            {
                                flights = flights.Where(s => s.ArrivalDate == arrivalDate);
                            }
                            break;
                        case "price":
                            if (decimal.TryParse(value, out decimal price))
                            {
                                flights = flights.Where(s => s.Price == price);
                            }
                            break;
                        case "transfers":
                            if (int.TryParse(value, out int transfersCount))
                            {
                                flights = flights.Where(s => s.Transfers == transfersCount);
                            }
                            break;
                    }
                }

            }

        }

        #endregion

        #region Sort flights

        private void SortFlights(string sortOrder, ref IEnumerable<FlightDto> flights)
        {
            _logger.LogInformation($"Sorting flights.");

            switch (sortOrder)
            {
                case "destination":
                    flights = flights.OrderBy(s => s.Destination);
                    break;
                case "destination_desc":
                    flights = flights.OrderByDescending(s => s.Destination);
                    break;
                case "departureDate":
                    flights = flights.OrderBy(s => s.DepartureDate);
                    break;
                case "departureDate_desc":
                    flights = flights.OrderByDescending(s => s.DepartureDate);
                    break;
                case "arrivalDate":
                    flights = flights.OrderBy(s => s.ArrivalDate);
                    break;
                case "arrivalDate_desc":
                    flights = flights.OrderByDescending(s => s.ArrivalDate);
                    break;
                case "airline":
                    flights = flights.OrderBy(s => s.Airline);
                    break;
                case "airline_desc":
                    flights = flights.OrderByDescending(s => s.Airline);
                    break;
                case "price":
                    flights = flights.OrderBy(s => s.Price);
                    break;
                case "price_desc":
                    flights = flights.OrderByDescending(s => s.Price);
                    break;
                case "transfers":
                    flights = flights.OrderBy(s => s.Transfers);
                    break;
                case "transfers_desc":
                    flights = flights.OrderByDescending(s => s.Transfers);
                    break;
                case "origin_desc":
                    flights = flights.OrderByDescending(s => s.Origin);
                    break;
                case "origin":
                default:
                    flights = flights.OrderBy(s => s.Origin);
                    break;
            }
        }

        #endregion

        #region Cache methods

        private void SetFlightDataToCache(string key, IEnumerable<FlightDto> flights)
        {
            _logger.LogInformation($"Save flights to cache.");

            var cacheOptions = new MemoryCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(_settings.CacheExpirationTimeInSec)
            };

            _cache.Set(key, flights, cacheOptions);
        }

        private IEnumerable<FlightDto>? GetFlightsFromCache(string key, ref bool isNeedToCached)
        {
            bool ok = _cache.TryGetValue(key, out object? data);

            if (ok == false)
            {
                var cacheOptions = new MemoryCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(_settings.CacheExpirationTimeInSec)
                };

                // save count for the first request
                _cache.Set(key, 1, cacheOptions);
            }
            else
            {
                if (data is int count)
                {
                    // update count of the same requests
                    // if count exceeds the threshold then save flights to cache
                    if (count >= _settings.Threshold)
                    {
                        isNeedToCached = true;
                    }
                    else
                    {
                        _cache.Set(key, ++count);
                    }
                }
                else
                {
                    _logger.LogInformation("Receive flights from cache.");
                    return (IEnumerable<FlightDto>)data;
                }
            }

            return null;
        }

        #endregion
    }
}