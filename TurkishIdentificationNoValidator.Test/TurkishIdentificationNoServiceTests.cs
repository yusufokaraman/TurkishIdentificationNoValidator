using Moq;
using TurkishIdentificationNoValidator.Services.Abstract;
using TurkishIdentificationNoValidator.Services;

namespace TurkishIdentificationNoValidator.Test
{
    [TestFixture]
    public class TurkishIdentificationNoServiceTests
    {
        [Test]
        public async Task Dogrula_ValidIdAndInternetConnection_ReturnsValidatedMessage()
        {
            // Arrange
            var mockPingService = new Mock<IPingService>();
            mockPingService.Setup(p => p.IsHostReachable(It.IsAny<string>())).Returns(true);

            var mockSoapClient = new Mock<IKPSPublicSoapClient>();
            mockSoapClient.Setup(s => s.TCKimlikNoDogrulaAsync(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
                .ReturnsAsync(new TCKimlikNoDogrulaResponse { Body = new TCKimlikNoDogrulaResponseBody { TCKimlikNoDogrulaResult = true } });

            var validator = new TurkishIdValidator(mockPingService.Object, mockSoapClient.Object);
            var service = new TurkishIdentificationNoService("12345678901", "Ad", "Soyad", 1980);

            // Act
            var result = await service.Dogrula();

            // Assert
            Assert.AreEqual("TC Kimlik Numarası hem algoritma hem de SOAP API üzerinden doğrulandı.", result);
        }

        [Test]
        public async Task Dogrula_ValidIdNoInternetConnection_ReturnsAlgorithmValidatedMessage()
        {
            // Arrange
            var mockPingService = new Mock<IPingService>();
            mockPingService.Setup(p => p.IsHostReachable(It.IsAny<string>())).Returns(false);

            var mockSoapClient = new Mock<IKPSPublicSoapClient>();
            var validator = new TurkishIdValidator(mockPingService.Object, mockSoapClient.Object);
            var service = new TurkishIdentificationNoService("12345678901", "Ad", "Soyad", 1980);

            // Act
            var result = await service.Dogrula();

            // Assert
            Assert.AreEqual("İnternet bağlantısı yok, TC Kimlik Numarası sadece algoritma ile doğrulandı.", result);
        }

        [Test]
        public async Task Dogrula_InvalidId_ReturnsInvalidIdMessage()
        {
            // Arrange
            var mockPingService = new Mock<IPingService>();
            var mockSoapClient = new Mock<IKPSPublicSoapClient>();
            var validator = new TurkishIdValidator(mockPingService.Object, mockSoapClient.Object);
            var service = new TurkishIdentificationNoService("1234567890", "Ad", "Soyad", 1980);

            // Act
            var result = await service.Dogrula();

            // Assert
            Assert.AreEqual("Geçersiz TC Kimlik Numarası.", result);
        }
    }
}
