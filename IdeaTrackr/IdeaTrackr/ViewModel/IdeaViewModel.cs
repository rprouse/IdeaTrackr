using IdeaTrackr.Helpers;
using IdeaTrackr.Interfaces;
using IdeaTrackr.Model;
using Microsoft.WindowsAzure.MobileServices;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace IdeaTrackr.ViewModel
{
    public class IdeaViewModel : BaseViewModel
    {
        Idea _idea;

        public IdeaViewModel(INavigation navigation, Idea idea)
        {
            Idea = idea;

            SaveCommand = new Command(async () =>
            {
                await AddIdeaCommandAsync();
                // TODO: Add to the list on the list view
                await navigation.PopAsync();
            });

            DeleteCommand = new Command(async () =>
            {
                // TODO: 
                await navigation.PopAsync();
            });

            CancelCommand = new Command(() =>
            {
                navigation.PopAsync();
            });
        }

        public Idea Idea
        {
            get { return _idea; }
            set { SetProperty(ref _idea, value); }
        }

        public ICommand SaveCommand { get; }

        public ICommand DeleteCommand { get; }

        public ICommand CancelCommand { get; }

        async Task AddIdeaCommandAsync()
        {
            if (IsBusy)
                return;

            try
            {
                if (!Settings.IsLoggedIn)
                {
                    await App.AzureService.Initialize();
                    var user = await DependencyService.Get<IAuthentication>().LoginAsync(App.AzureService.MobileService, MobileServiceAuthenticationProvider.Twitter);
                    if (user == null)
                        return;
                }
                IsBusy = true;
                //Xamarin.Insights.Track("IdeaAdded");

                await App.AzureService.AddIdea(Idea);
                //Ideas.Add(idea);
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
