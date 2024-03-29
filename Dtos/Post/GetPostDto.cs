using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using reddit_clone.Models;
using reddit_clone_backend.Dtos.Comment;
using reddit_clone_backend.Models;
using static reddit_clone.Models.Community;

namespace reddit_clone.Dtos.Post
{
    public class GetPostDto
    {

        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public DateTime CreatedTime { get; set; }
        public int TotalVote {get; set; } = 0;
        public int? CommunityId { get; set; }
        public string? CommunityName { get; set; }
        public string? Username { get; set; }
        public bool IsSaved { get; set; } = false;
        public int CommentCount { get; set; } = 0;
        public List<GetCommentDto>? Comments { get; set; } = new List<GetCommentDto>();


        public GetPostDto(Models.Post post, bool isSaved, string userId)
        {
            Id = post.Id;
            Title = post.Title;
            Content = post.Content;
            CreatedTime = post.CreatedTime;
            CommunityId = post.CommunityId;
            CommunityName = post.Community != null ? post.Community.Title : "not found";
            Username = post.User != null ? post.User.UserName : "not found";
            IsSaved = isSaved;
            CommentCount = post.Comments.Count();
            TotalVote = post.TotalVoteCount;
        }

        public GetPostDto(Models.Post post)
        {
            Id = post.Id;
            Title = post.Title;
            Content = post.Content;
            CreatedTime = post.CreatedTime;
            CommunityId = post.CommunityId;
            CommunityName = post.Community != null ? post.Community.Title : "not found";
            Username = post.User != null ? post.User.UserName : "not found";
            CommentCount = post.Comments.Count();
            TotalVote = post.TotalVoteCount;
        }

        public GetPostDto() { }
    }
}