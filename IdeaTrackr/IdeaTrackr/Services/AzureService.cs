using IdeaTrackr.Helpers;
using IdeaTrackr.Model;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace IdeaTrackr.Services
{
    public class AzureService
    {
        public MobileServiceClient MobileService { get; set; }
        IMobileServiceSyncTable<Idea> _ideaTable;
        bool _initialized;

        public async Task Initialize()
        {
            if (_initialized)
                return;

            //Create our client
            var handler = new AuthHandler();
            MobileService = new MobileServiceClient("https://ideatrackr.azurewebsites.net", handler);
            handler.Client = MobileService;

            if (!string.IsNullOrWhiteSpace(Settings.AuthToken) && !string.IsNullOrWhiteSpace(Settings.UserId))
            {
                MobileService.CurrentUser = new MobileServiceUser(Settings.UserId);
                MobileService.CurrentUser.MobileServiceAuthenticationToken = Settings.AuthToken;
            }

            const string path = "syncstore.db";
            //setup our local sqlite store and intialize our table
            var store = new MobileServiceSQLiteStore(path);
            store.DefineTable<Idea>();

            await MobileService.SyncContext.InitializeAsync(store, new MobileServiceSyncHandler());

            //Get our sync table that will call out to azure
            _ideaTable = MobileService.GetSyncTable<Idea>();

            _initialized = true;
        }

        public async Task<IList<Idea>> GetIdeas()
        {
            await Initialize();
            await SyncIdeas();
            return await GetSyncedIdeas();
        }

        public async Task<IList<Idea>> GetSyncedIdeas()
        {
            await Initialize();
            return await _ideaTable.OrderByDescending(i => i.UpdatedAt).ToListAsync();
        }

        public async Task<Idea> AddIdea(Idea idea)
        {
            idea.UpdatedAt = DateTime.UtcNow;
            if (string.IsNullOrWhiteSpace(idea.Id))
            {
                idea.CreatedAt = DateTime.UtcNow;
                await _ideaTable.InsertAsync(idea);
            }
            else
            {
                await _ideaTable.UpdateAsync(idea);
            }
            await SyncIdeas();
            return idea;
        }

        public async Task SyncIdeas()
        {
            try
            {
                // TODO: In the future, just pull ideas that are changed since the last update?
                await _ideaTable.PullAsync("allIdeas", _ideaTable.CreateQuery());
                await MobileService.SyncContext.PushAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to sync ideas at this time, going offline: " + ex);
            }
        }
    }
}
