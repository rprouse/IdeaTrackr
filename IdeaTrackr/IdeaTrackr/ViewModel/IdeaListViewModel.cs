﻿using FormsToolkit;
using IdeaTrackr.Model;
using IdeaTrackr.Services;
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
    public class IdeaListViewModel : BaseViewModel
    {
        ICommand _loadIdeasCommand;
        ICommand _addIdeaCommand;
        AzureService _service = new AzureService();

        public ObservableRangeCollection<Idea> Ideas { get; } = new ObservableRangeCollection<Idea>();
        public ObservableRangeCollection<Grouping<string, Idea>> IdeasGrouped { get; } = new ObservableRangeCollection<Grouping<string, Idea>>();

        string loadingMessage;
        public string LoadingMessage
        {
            get { return loadingMessage; }
            set { SetProperty(ref loadingMessage, value); }
        }
        
        public ICommand LoadIdeasCommand =>
            _loadIdeasCommand ?? (_loadIdeasCommand = new Command(async () => await ExecuteLoadIdeasCommandAsync()));

        async Task ExecuteLoadIdeasCommandAsync()
        {
            if (IsBusy)
                return;

            try
            {
                //if (!Settings.IsLoggedIn)
                //{
                //    await azureService.Initialize();
                //    var user = await DependencyService.Get<IAuthentication>().LoginAsync(azureService.MobileService, MobileServiceAuthenticationProvider.MicrosoftAccount);
                //    if (user == null)
                //        return;
                //}
                
                LoadingMessage = "Loading Ideas...";
                IsBusy = true;
                var ideas = await _service.GetIdeas();
                Ideas.ReplaceRange(ideas);
                SortIdeas();
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

        void SortIdeas()
        {
            var groups = from idea in Ideas
                         orderby idea.DateUtc descending
                         group idea by idea.Status
                into ideaGroup
                         select new Grouping<string, Idea>($"{ideaGroup.Key} ({ideaGroup.Count()})", ideaGroup);


            IdeasGrouped.ReplaceRange(groups);
        }

        public ICommand AddIdeaCommand =>
            _addIdeaCommand ?? (_addIdeaCommand = new Command(async () => await ExecuteAddIdeaCommandAsync()));

        async Task ExecuteAddIdeaCommandAsync()
        {
            //if (IsBusy)
            //    return;

            //try
            //{
            //    if (!Settings.IsLoggedIn)
            //    {
            //        await azureService.Initialize();
            //        var user = await DependencyService.Get<IAuthentication>().LoginAsync(azureService.MobileService, MobileServiceAuthenticationProvider.MicrosoftAccount);
            //        if (user == null)
            //            return;

            //        LoadingMessage = "Adding Idea...";
            //        IsBusy = true;

            //        var ideas = await azureService.GetIdeas();
            //        Ideas.ReplaceRange(ideas);

            //        SortIdeas();
            //    }
            //    else
            //    {
            //        LoadingMessage = "Adding Idea...";
            //        IsBusy = true;
            //    }
            //    Xamarin.Insights.Track("IdeaAdded");

            //    var coffee = await azureService.AddIdea(AtHome);
            //    Ideas.Add(coffee);
            //    SortIdeas();
            //}
            //catch (Exception ex)
            //{
            //    Debug.WriteLine(ex);
            //    Xamarin.Insights.Report(ex);
            //}
            //finally
            //{
            //    IsBusy = false;
            //}
        }
    }
}
