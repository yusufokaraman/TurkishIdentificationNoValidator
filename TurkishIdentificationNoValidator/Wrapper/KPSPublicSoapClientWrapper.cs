using TurkishIdentificationNoValidator.Services.Abstract;

namespace TurkishIdentificationNoValidator.Wrapper
{
    public class KPSPublicSoapClientWrapper : IKPSPublicSoapClient
    {
        private readonly KPSPublicSoapClient _client;

        public KPSPublicSoapClientWrapper(KPSPublicSoapClient client)
        {
            _client = client;
        }

        public Task<TCKimlikNoDogrulaResponse> TCKimlikNoDogrulaAsync(long tcKimlikNo, string ad, string soyad, int dogumYili)
        {
            return _client.TCKimlikNoDogrulaAsync(tcKimlikNo, ad, soyad, dogumYili);
        }
    }
}
