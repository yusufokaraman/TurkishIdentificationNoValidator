using System.Net.NetworkInformation;
using TurkishIdentificationNoValidator.Services.Abstract;

namespace TurkishIdentificationNoValidator.Services.Concrete
{
    public class MockPingService : IPingService
    {
        public bool IsHostReachable(string host)
        {
            try
            {
                using (var ping = new Ping())
                {
                    PingReply reply = ping.Send(host);
                    return reply.Status == IPStatus.Success;
                }
            }
            catch
            {
                return false;
            }
        }
    }

}
