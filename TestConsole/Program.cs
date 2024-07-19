using TurkishIdentificationNoValidator.Services;

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

        var service = new TurkishIdentificationNoService(tcKimlikNo, ad, soyad, dogumYili);
        string result = await service.Dogrula();

        Console.WriteLine(result);
    }

}
