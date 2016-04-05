using FormsToolkit;
using IdeaTrackr.Helpers;
using IdeaTrackr.Model;
using IdeaTrackr.View;
using MvvmHelpers;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace IdeaTrackr.ViewModel
{
    public class IdeaListViewModel : BaseViewModel
    {
        ICommand _refreshIdeasCommand;
        string _loadingMessage;

        INavigation _navigation;
        public ObservableRangeCollection<Idea> Ideas { get; } = new ObservableRangeCollection<Idea>();

        public IdeaListViewModel(INavigation navigation)
        {
            _navigation = navigation;
        }

        public string LoadingMessage
        {
            get { return _loadingMessage; }
            set { SetProperty(ref _loadingMessage, value); }
        }

        public ICommand RefreshIdeas =>
            _refreshIdeasCommand ?? (_refreshIdeasCommand = new Command(async () => {
                if (IsBusy) return;
                IsBusy = true;
                await RefreshIdeasAsync();
                IsBusy = false;
            }));

        public async Task LoadIdeasAsync()
        {
            if (IsBusy)
                return;

            try
            {
                if (!await Authentication.AttemptLogin())
                    return;

                LoadingMessage = "Loading Ideas...";
                IsBusy = true;
                var ideas = await App.AzureService.GetIdeas();
                Ideas.ReplaceRange(ideas);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                MessagingService.Current.SendMessage("message", new MessagingServiceAlert
                {
                    Cancel = "OK",
                    Message = "Unable to sync ideas, you may be offline",
                    Title = "Idea sync Error"
                });
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task RefreshIdeasAsync()
        {
            try
            {
                if (!await Authentication.AttemptLogin())
                    return;

                var ideas = await App.AzureService.GetIdeas();
                Ideas.ReplaceRange(ideas);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public async Task AddIdea()
        {
            await ShowIdeaView(new Idea());
        }

        public async Task ShowIdeaView(Idea idea)
        {
            var ideaPage = new IdeaView(idea);
            await _navigation.PushAsync(ideaPage);
        }
    }
}
