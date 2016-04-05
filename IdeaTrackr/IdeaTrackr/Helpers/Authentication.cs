using IdeaTrackr.Interfaces;
using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace IdeaTrackr.Helpers
{
    public static class Authentication
    {
        public static async Task<bool> AttemptLogin()
        {
            if (!Settings.IsLoggedIn)
            {
                await App.AzureService.Initialize();
                var user = await DependencyService.Get<IAuthentication>().LoginAsync(App.AzureService.MobileService, MobileServiceAuthenticationProvider.Twitter);
                if (user == null)
                    return false;
            }
            return true;
        }
    }
}
