namespace TurkishIdentificationNoValidator.Services.Abstract
{
    public interface IKPSPublicSoapClient
    {
        Task<TCKimlikNoDogrulaResponse> TCKimlikNoDogrulaAsync(long tcKimlikNo, string ad, string soyad, int dogumYili);
    }
}
