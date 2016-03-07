using IdeaTrackr.Model;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.Sync;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdeaTrackr.Services
{
    public class AzureService
    {
        private IList<Idea> _ideas;

        public AzureService()
        {
            _ideas = new List<Idea>
            {
                new Idea
                {
                    Id = Guid.NewGuid().ToString("N"),
                    DateUtc = DateTime.UtcNow,
                    Name = "Training Plan Generator",
                    Description = "Collect data and automatically generate a  cycling training plan, then track progress towards the plan and goals. Once established, branch out to running, then other sports.",
                    Status = Status.Good
                },
                new Idea
                {
                    Id = Guid.NewGuid().ToString("N"),
                    DateUtc = DateTime.UtcNow,
                    Name = "MyNextSkis.com",
                    Description = "Website/App that lists all skis from all manufacturers and creates advanced searches",
                    Status = Status.Good
                },
                new Idea
                {
                    Id = Guid.NewGuid().ToString("N"),
                    DateUtc = DateTime.UtcNow,
                    Name = "Pay as you go gym",
                    Description = "Website to find small independent gyms and pay for one time visits",
                    Status = Status.Bad
                },
                new Idea
                {
                    Id = Guid.NewGuid().ToString("N"),
                    DateUtc = DateTime.UtcNow,
                    Name = "Safe Beaches",
                    Description = "When you arrive at the beach, checks if it is safe to swim and notifies you. Also notifies you when reading change for your favorite beaches",
                    Status = Status.None
                },
                new Idea
                {
                    Id = Guid.NewGuid().ToString("N"),
                    DateUtc = DateTime.UtcNow,
                    Name = "Museum App",
                    Description = "Receive notifications of new exhibits. Arrive and am shown a list of exhibits, pick one and I am led on a personal guided tour by my phone",
                    Status = Status.Good
                }
            };
        }

        public MobileServiceClient MobileService { get; set; }
        IMobileServiceSyncTable _ideaTable;

        public async Task Initialize()
        {
        }

        public async Task<IList<Idea>> GetIdeas()
        {
            return _ideas;
        }

        public async Task AddIdea(Idea idea)
        {
            if (string.IsNullOrWhiteSpace(idea.Id))
            {
                idea.Id = Guid.NewGuid().ToString("N");
                idea.DateUtc = DateTime.UtcNow;
            }
            else
            {
                int i = _ideas.IndexOf(idea);
                _ideas.RemoveAt(i);
            }
            _ideas.Insert(0, idea);
        }

        public async Task SyncIdeas()
        {
        }
    }
}
