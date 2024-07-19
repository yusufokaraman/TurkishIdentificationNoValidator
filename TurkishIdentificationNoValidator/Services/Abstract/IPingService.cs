namespace TurkishIdentificationNoValidator.Services.Abstract
{
    public interface IPingService
    {
        bool IsHostReachable(string host);
    }
}
