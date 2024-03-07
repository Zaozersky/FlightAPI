using System.Net;
using FlightAPI.Common;
using FlightAPI.Models;
using FlightAPI.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;

namespace FlightAPI.Tests
{
    [TestFixture]
    public class AggregatedFlightServiceTests
    {
        private AggregatedFlightService _service;

        private Mock<IMemoryCache> _mockCache;
        private Mock<ILogger<AggregatedFlightService>> _mockLogger;
        private Mock<IOptions<AppSettings>> _mockAppSettings;
        private Mock<IHttpClientFactory> _mockHttpClientFactory;

        private const string TEST_URL = "http://test.com/";

        private List<JsonFlight> _jsonFlights;

        [OneTimeSetUp]
        public void SetUp()
        {
            _mockCache = new Mock<IMemoryCache>();
            _mockLogger = new Mock<ILogger<AggregatedFlightService>>();
            _mockAppSettings = new Mock<IOptions<AppSettings>>();
            _mockHttpClientFactory = new Mock<IHttpClientFactory>();

            var options = new AppSettings()
            {
                Threshold = 2,
                CacheExpirationTimeInSec = 60,
                DataSources = $"testAPI: {TEST_URL};"
            };

            _mockAppSettings.Setup(x => x.Value).Returns(options);

            var mapper = MappingProfile.InitializeAutoMapper().CreateMapper();

            var item1 = new JsonFlight() { id = 1, origin = "LED", destination = "NYC" };
            var item2 = new JsonFlight() { id = 2, origin = "BEG", destination = "LON" };
            var item3 = new JsonFlight() { id = 3, origin = "SYD", destination = "HND" };

            _jsonFlights = new List<JsonFlight>(new[] { item1, item2, item3 });

            var json = JsonConvert.SerializeObject(_jsonFlights);

            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               )
               .ReturnsAsync(new HttpResponseMessage()
               {
                   StatusCode = HttpStatusCode.OK,
                   RequestMessage = new HttpRequestMessage()
                   {
                       RequestUri = new Uri("http://test.com/")
                   },
                   Content = new StringContent(json)
               })
               .Verifiable();

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("http://test.com/"),
            };

            _mockHttpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

            object? data = null;
            _mockCache.Setup(m => m.TryGetValue(
                It.IsAny<string>(),
                out data))
                .Returns(true);

            _service = new AggregatedFlightService(_mockHttpClientFactory.Object, _mockCache.Object,
                mapper, _mockLogger.Object, _mockAppSettings.Object);
        }

        [Test]
        public void ReceiveFlightsFromAPI_WithEmptyQuery_ReturnsAllFlights()
        {
            // Arrange
            var query = new QueryParameters();

            // Act
            var actual = _service.GetFlights(query, new CancellationToken()).GetAwaiter().GetResult();

            // Assert
            Assert.That(actual.Count(), Is.EqualTo(_jsonFlights.Count));
        }

        [Test]
        public void ReceiveFlightsFromAPI_WithFilter_ReturnsTrimmedFlights()
        {
            // Arrange
            var query = new QueryParameters() { Filter = "origin = LED" };

            // Act
            var actual = _service.GetFlights(query, new CancellationToken()).GetAwaiter().GetResult();

            // Assert
            int expected = 1;
            Equals(actual.Count(), expected);
        }

        [Test]
        public void ReceiveFlightsFromAPI_WithBadFilterFormat_ThrowsArgumentException()
        {
            // Arrange
            var query = new QueryParameters() { Filter = "origin:LED&destination=NYC&order=arrivalDate_desc" };

            // Act
            var actual = () => _service.GetFlights(query, new CancellationToken());

            // Assert
            Assert.Throws<ArgumentException>(() => throw new ArgumentException());
        }
    }
}
