using System;
using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;
using IdeaTrackr.Helpers;
using IdeaTrackr.Interfaces;
using Xamarin.Forms;
using IdeaTrackr.UWP.Helpers;

[assembly: Dependency(typeof(Authentication))]
namespace IdeaTrackr.UWP.Helpers
{
    public class Authentication : IAuthentication
    {
        public async Task<MobileServiceUser> LoginAsync(MobileServiceClient client, MobileServiceAuthenticationProvider provider)
        {
            try
            {
                var user = await client.LoginAsync(provider);

                Settings.AuthToken = user?.MobileServiceAuthenticationToken ?? string.Empty;
                Settings.UserId = user?.UserId ?? string.Empty;

                return user;
            }
            catch (Exception e)
            {
                //e.Data["method"] = "LoginAsync";
                //Xamarin.Insights.Report(e);
            }

            return null;
        }

        public void ClearCookies()
        {
        }
    }
}
