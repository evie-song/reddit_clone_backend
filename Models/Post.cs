using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using reddit_clone_backend.Models;

namespace reddit_clone.Models
{
    public class Post
    {
        public int Id {get; private set;}
        public string? Title {get; set;}
        public string? Content {get; set;}
        public DateTime CreatedTime { get; private set; }
        public int? CommunityId {get; set;}
        public Community? Community {get; set;}
        public string? UserId { get; set; }
        public IdentityUser? User {get; set;}
        public string? ApplicationUserId { get; set; }
        public ApplicationUser? ApplicationUser { get;  set; }
        public List<SavedPost> SavedPosts { get; set; } = new List<SavedPost>();
        public List<VoteRegistration> VoteRegistrations { get; set; } = new List<VoteRegistration>();
        public List<Comment> Comments {get; set; } = new List<Comment>();

        public Post() {
            CreatedTime = DateTime.Now;
        }

        public int TotalVoteCount {
            get {
                int totalVote = 0;
                foreach (var voteRegistration in VoteRegistrations)
                {
                    totalVote += (int)voteRegistration.VoteValue;
                }
                return totalVote;
            } 
        }
    }
}

