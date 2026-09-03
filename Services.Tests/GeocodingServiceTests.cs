using Data;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.Protected;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Services.Tests
{
    public class GeocodingServiceTests
    {
        [Fact]
        public async Task GetCoordinates_DebeDevolverLasCoordenadasAsync()
        {
            // Arrange
            var json = """
        [
            {
                "lat": "-34.6037",
                "lon": "-58.3816"
            }
        ]
        """;

            var handlerMock = new Mock<HttpMessageHandler>();

            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(json)
                });

            var httpClient = new HttpClient(handlerMock.Object);

            var service = new GeocodingService(httpClient);

            // Act
            var resultado = await service.GetCoordinates("Buenos Aires");

            // Assert
            Assert.Equal(-34.6037, resultado.lat);
            Assert.Equal(-58.3816, resultado.lon);

        }

        [Fact]
        public async Task GetCoordinates_DireccionNoEncontrada_DebeLanzarException()
        {
            // Arrange
            var json = "[]";

            var handlerMock = new Mock<HttpMessageHandler>();

            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(json)
                });

            var httpClient = new HttpClient(handlerMock.Object);

            var service = new GeocodingService(httpClient);

            // Act
            var exception = await Assert.ThrowsAsync<Exception>(
                () => service.GetCoordinates("Direccion que no existe")
            );

            // Assert
            Assert.Equal("Dirección no encontrada", exception.Message);
        }
    }
}
