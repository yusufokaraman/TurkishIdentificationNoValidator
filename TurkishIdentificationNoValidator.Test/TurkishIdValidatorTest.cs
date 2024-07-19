using Moq;
using TurkishIdentificationNoValidator.Services.Abstract;
using Xunit;
using Assert = Xunit.Assert;

namespace TurkishIdentificationNoValidator.Test
{
    public class TurkishIdValidatorTest
    {
        [Fact]
        public void TcKimlikNoKontrol_ShouldReturnTrue_ForValidTcKimlikNo()
        {
            var pingServiceMock = new Mock<IPingService>();
            var soapClientMock = new Mock<IKPSPublicSoapClient>();
            var validator = new TurkishIdValidator(pingServiceMock.Object, soapClientMock.Object);
            bool result = validator.TcKimlikNoKontrol("10000000146");
            Assert.True(result);
        }

        [Fact]
        public void TcKimlikNoKontrol_ShouldReturnFalse_ForInvalidTcKimlikNo()
        {
            var pingServiceMock = new Mock<IPingService>();
            var soapClientMock = new Mock<IKPSPublicSoapClient>();
            var validator = new TurkishIdValidator(pingServiceMock.Object, soapClientMock.Object);
            bool result = validator.TcKimlikNoKontrol("1234567890");
            Assert.False(result);
        }

        [Fact]
        public void CheckInternetConnection_ShouldReturnFalse_WhenNoInternetConnection()
        {
            var pingServiceMock = new Mock<IPingService>();
            var soapClientMock = new Mock<IKPSPublicSoapClient>();
            pingServiceMock.Setup(p => p.IsHostReachable(It.IsAny<string>())).Returns(false);
            var validator = new TurkishIdValidator(pingServiceMock.Object, soapClientMock.Object);
            bool result = validator.CheckInternetConnection();
            Assert.False(result);
        }

        [Fact]
        public async Task TcKimlikNoApiDogrula_ShouldThrowInvalidOperationException_WhenNoInternetConnection()
        {
            var pingServiceMock = new Mock<IPingService>();
            var soapClientMock = new Mock<IKPSPublicSoapClient>();
            pingServiceMock.Setup(p => p.IsHostReachable(It.IsAny<string>())).Returns(false);
            var validator = new TurkishIdValidator(pingServiceMock.Object, soapClientMock.Object);
            await Assert.ThrowsAsync<InvalidOperationException>(() => validator.TcKimlikNoApiDogrula(12345678901, "Ad", "Soyad", 1990));
        }

        [Fact]
        public async Task TcKimlikNoApiDogrula_ShouldReturnTrue_ForValidTcKimlikNo()
        {
            var pingServiceMock = new Mock<IPingService>();
            var soapClientMock = new Mock<IKPSPublicSoapClient>();
            pingServiceMock.Setup(p => p.IsHostReachable(It.IsAny<string>())).Returns(true);
            soapClientMock
                .Setup(client => client.TCKimlikNoDogrulaAsync(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
                .ReturnsAsync(new TCKimlikNoDogrulaResponse(new TCKimlikNoDogrulaResponseBody { TCKimlikNoDogrulaResult = true }));

            var validator = new TurkishIdValidator(pingServiceMock.Object, soapClientMock.Object);

            bool result = await validator.TcKimlikNoApiDogrula(12345678901, "Ad", "Soyad", 1990);
            Assert.True(result);
        }
    }
}
