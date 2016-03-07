using IdeaTrackr.Services;
using IdeaTrackr.View;

using Xamarin.Forms;

namespace IdeaTrackr
{
    public class App : Application
    {
        public static AzureService AzureService { get; } = new AzureService();

        public App()
        {
            // The root page of your application
            MainPage = new NavigationPage(new IdeaListView());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
