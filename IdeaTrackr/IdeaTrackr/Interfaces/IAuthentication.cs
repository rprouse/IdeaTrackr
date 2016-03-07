using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;

namespace IdeaTrackr.Interfaces
{
    public interface IAuthentication
    {
        Task<MobileServiceUser> LoginAsync(MobileServiceClient client, MobileServiceAuthenticationProvider provider);

        void ClearCookies();
    }
}
