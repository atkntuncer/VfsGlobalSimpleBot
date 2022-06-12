using System.Threading.Tasks;

namespace CoreConsoleTemplate.Bussines
{
    public interface ISendRequest
    {
        Task<bool> CheckAppointment();
    }
}