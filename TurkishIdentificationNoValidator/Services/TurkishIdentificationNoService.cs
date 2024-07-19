using TurkishIdentificationNoValidator.Services.Abstract;
using TurkishIdentificationNoValidator.Services.Concrete;
using TurkishIdentificationNoValidator.Wrapper;

namespace TurkishIdentificationNoValidator.Services
{
    public class TurkishIdentificationNoService
    {
        private readonly TurkishIdValidator _validator;

        public TurkishIdentificationNoService(string tcKimlikNo, string ad, string soyad, int dogumYili)
        {
            IPingService pingService = new MockPingService();
            IKPSPublicSoapClient soapClient = new KPSPublicSoapClientWrapper(new KPSPublicSoapClient(KPSPublicSoapClient.EndpointConfiguration.KPSPublicSoap));
            _validator = new TurkishIdValidator(pingService, soapClient);

            TcKimlikNo = tcKimlikNo;
            Ad = ad;
            Soyad = soyad;
            DogumYili = dogumYili;
        }

        public string TcKimlikNo { get; }
        public string Ad { get; }
        public string Soyad { get; }
        public int DogumYili { get; }

        public async Task<string> Dogrula()
        {
            if (_validator.TcKimlikNoKontrol(TcKimlikNo))
            {
                if (_validator.CheckInternetConnection())
                {
                    bool apiResult = await _validator.TcKimlikNoApiDogrula(long.Parse(TcKimlikNo), Ad, Soyad, DogumYili);
                    if (apiResult)
                    {
                        return "TC Kimlik Numarası hem algoritma hem de SOAP API üzerinden doğrulandı.";
                    }
                    else
                    {
                        return "TC Kimlik Numarası algoritma ile geçerli, ancak SOAP API üzerinden doğrulanamadı.";
                    }
                }
                else
                {
                    return "İnternet bağlantısı yok, TC Kimlik Numarası sadece algoritma ile doğrulandı.";
                }
            }
            else
            {
                return "Geçersiz TC Kimlik Numarası.";
            }
        }
    }
}
