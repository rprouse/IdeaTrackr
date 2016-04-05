using IdeaTrackr.Helpers;
using IdeaTrackr.Interfaces;
using IdeaTrackr.Model;
using Microsoft.WindowsAzure.MobileServices;
using MvvmHelpers;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace IdeaTrackr.ViewModel
{
    public class IdeaViewModel : BaseViewModel
    {
        INavigation _navigation;
        Idea _idea;

        public IdeaViewModel(INavigation navigation, Idea idea)
        {
            _navigation = navigation;
            Idea = idea;
        }

        public Idea Idea
        {
            get { return _idea; }
            set { SetProperty(ref _idea, value); }
        }

        public async Task SaveIdeaAsync()
        {
            if (IsBusy)
                return;

            try
            {
                if (!await Authentication.AttemptLogin())
                    return;

                IsBusy = true;
                //Xamarin.Insights.Track("IdeaAdded");

                await App.AzureService.AddIdea(Idea);
                await _navigation.PopAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                //Xamarin.Insights.Report(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
