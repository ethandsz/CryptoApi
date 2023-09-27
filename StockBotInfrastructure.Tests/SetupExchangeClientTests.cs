using System.Security.Authentication;
using Bybit.Net.Clients;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using StockBotDomain.Models;

namespace StockBotInfrastructure.Tests
{
    [TestFixture]
    public class SetupExchangeClientTests
    {
        [Test]
        public void SetupByBitClient_ValidClient_Success()
        {
            
            // Arrange
            var clientConfiguration = new ClientConfiguration
            {
                ApiKey = "your-api-key",
                ApiSecret = "your-api-secret"
            };

            var mockSetup = Substitute.For<ISetupExchangeClient>();

            mockSetup.SetupByBitClient(clientConfiguration).Returns(new BybitRestClient());

            //Act
            var result = mockSetup.SetupByBitClient(clientConfiguration);
    
            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void SetupByBitClient_InvalidClient_ThrowsException()
        {
            
            // Arrange
            var clientConfiguration = new ClientConfiguration
            {
                ApiKey = "invalid-api-key",
                ApiSecret = "invalid-api-secret"
            };

            var mockSetup = Substitute.For<ISetupExchangeClient>();

            mockSetup.SetupByBitClient(clientConfiguration).Throws<InvalidCredentialException>();

            //Act Assert
            Assert.Throws<InvalidCredentialException>(() =>
            {
                mockSetup.SetupByBitClient(clientConfiguration);
            });
        }
    }
}