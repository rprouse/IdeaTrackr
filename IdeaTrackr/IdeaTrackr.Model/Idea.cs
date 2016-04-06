using System;

namespace IdeaTrackr.Model
{
    public class Idea
    {
        public string Id { get; set; }
        
        public string UserId { get; set; }

        public string Name { get; set; }

        public string Problem { get; set; }

        public string Solution { get; set; }

        public string Notes { get; set; }

        public Status Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
