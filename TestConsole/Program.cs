using TurkishIdentificationNo;

internal class Program
{
    static async Task Main(string[] args)
    {
        Console.Write("TC Kimlik No: ");
        string tcKimlikNo = Console.ReadLine();

        Console.Write("Ad: ");
        string ad = Console.ReadLine();

        Console.Write("Soyad: ");
        string soyad = Console.ReadLine();

        Console.Write("Doğum Yılı: ");
        int dogumYili = int.Parse(Console.ReadLine());

        if (TcKimlikNoKontrol(tcKimlikNo))
        {
            bool apiResult = await TcKimlikNoApiDogrula(long.Parse(tcKimlikNo), ad, soyad, dogumYili);
            if (apiResult)
            {
                Console.WriteLine("TC Kimlik Numarası doğrulandı.");
            }
            else
            {
                Console.WriteLine("TC Kimlik Numarası doğrulanamadı.");
            }
        }
        else
        {
            Console.WriteLine("Geçersiz TC Kimlik Numarası.");
        }
    }
    static bool TcKimlikNoKontrol(string tcKimlikNo)
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

        int tenthDigit = (oddSum * 7 - evenSum) % 10;
        int eleventhDigit = (oddSum + evenSum + digits[9]) % 10;

        return digits[9] == tenthDigit && digits[10] == eleventhDigit;
    }
    static async Task<bool> TcKimlikNoApiDogrula(long tcKimlikNo, string ad, string soyad, int dogumYili)
    {
        try
        {
            var client = new KPSPublicSoapClient(KPSPublicSoapClient.EndpointConfiguration.KPSPublicSoap);
            var result = await client.TCKimlikNoDogrulaAsync(tcKimlikNo, ad.ToUpper(), soyad.ToUpper(), dogumYili);

            return result.Body.TCKimlikNoDogrulaResult;
        }
        catch (Exception)
        {
            return false;
        }
    }
}