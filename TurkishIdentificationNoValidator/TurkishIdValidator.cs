using TurkishIdentificationNoValidator.Services.Abstract;

namespace TurkishIdentificationNoValidator
{
    public class TurkishIdValidator
    {
        private readonly IKPSPublicSoapClient _soapClient;
        private readonly IPingService _pingService;

        public TurkishIdValidator(IPingService pingService, IKPSPublicSoapClient soapClient)
        {
            _pingService = pingService;
            _soapClient = soapClient;
        }

        public bool TcKimlikNoKontrol(string tcKimlikNo)
        {
            if (tcKimlikNo.Length != 11 || !long.TryParse(tcKimlikNo, out _) || tcKimlikNo[0] == '0')
            {
                return false;
            }

            int[] digits = new int[11];
            for (int i = 0; i < 11; i++)
            {
                digits[i] = int.Parse(tcKimlikNo[i].ToString());
            }

            int oddSum = digits[0] + digits[2] + digits[4] + digits[6] + digits[8];
            int evenSum = digits[1] + digits[3] + digits[5] + digits[7];

            int tenthDigit = ((oddSum * 7) - evenSum) % 10;
            if (tenthDigit < 0)
            {
                tenthDigit += 10;
            }
            int eleventhDigit = (oddSum + evenSum + digits[9]) % 10;

            return digits[9] == tenthDigit && digits[10] == eleventhDigit;
        }


        public async Task<bool> TcKimlikNoApiDogrula(long tcKimlikNo, string ad, string soyad, int dogumYili)
        {
            if (!CheckInternetConnection())
            {
                throw new InvalidOperationException("İnternet bağlantısı yok.");
            }

            try
            {
                var result = await _soapClient.TCKimlikNoDogrulaAsync(tcKimlikNo, ad.ToUpper(), soyad.ToUpper(), dogumYili);
                return result.Body.TCKimlikNoDogrulaResult;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool CheckInternetConnection()
        {
            return _pingService.IsHostReachable("www.google.com");
        }
    }
}
