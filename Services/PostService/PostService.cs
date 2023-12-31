using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using reddit_clone.Data;

namespace reddit_clone.Services.PostService
{
    public class PostService : IPostService
    {

        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public PostService(IMapper mapper, DataContext context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<List<GetPostDto>>> AddPost(AddPostDto newPost)
        {
            var servicesResponse = new ServiceResponse<List<GetPostDto>>();

            var post = _mapper.Map<Post>(newPost);
            var community = await _context.Communities.FindAsync(newPost.CommunityId);
            post.Community = community;
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            var posts = await _context.Posts.Select(p => new GetPostDto(p)).ToListAsync();
            servicesResponse.Data = posts.OrderBy(post => post.Id).ToList();
            // servicesResponse.Data = _context.Posts.Select(p => _mapper.Map<GetPostDto>(p)).ToList();
            return servicesResponse;
        }

        async public Task<ServiceResponse<GetPostDto>> DecreaseVoteByOne(int id)
        {
            var servicesResponse = new ServiceResponse<GetPostDto>();
            try 
            {
                var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == id );
                if (post is null)
                    throw new Exception($"Charactor with Id `{id}` not found");
                post.DownVote += 1;
                await _context.SaveChangesAsync();

                // servicesResponse.Data = _mapper.Map<GetPostDto>(post);
                servicesResponse.Data = new GetPostDto(post);

            } catch(Exception ex) {
                servicesResponse.Success = false;
                servicesResponse.Message = ex.Message;
            }
            return servicesResponse;
        }

        public async Task<ServiceResponse<List<GetPostDto>>> DeletePost(int id)
        {
            var servicesResponse = new ServiceResponse<List<GetPostDto>>();

            try
            {
                var post = await _context.Posts.FindAsync(id);

                if (post is null)
                    throw new Exception($"Charactor with Id `{id}` not found");

                _context.Posts.Remove(post);

                await _context.SaveChangesAsync();

                servicesResponse.Data = _context.Posts.Select(p => _mapper.Map<GetPostDto>(p)).ToList();
            }
            catch (Exception ex)
            {   
                servicesResponse.Success = false;
                servicesResponse.Message = ex.Message;
            }
            return servicesResponse;
        }

        public async Task<ServiceResponse<List<GetPostDto>>> GetAllPost()
        {   
            var servicesResponse = new ServiceResponse<List<GetPostDto>>();
            var posts = await _context.Posts
                .Include(p => p.Community)
                .Select(p => new GetPostDto(p))
                .ToListAsync();
            servicesResponse.Data = posts.OrderBy(post => post.Id).ToList();
            return servicesResponse;
        }

        public async Task<ServiceResponse<GetPostDto>> GetPostById(int id)
        {   
            var servicesResponse = new ServiceResponse<GetPostDto>();
            var dbPost = await _context.Posts
                .Include(p => p.Community)
                .FirstOrDefaultAsync(p => p.Id == id );
            servicesResponse.Data = _mapper.Map<GetPostDto>(dbPost);
            return servicesResponse;
        }

        public async Task<ServiceResponse<GetPostDto>> IncreaseVoteByOne(int id) {
            var servicesResponse = new ServiceResponse<GetPostDto>();
            try 
            {
                var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == id );
                if (post is null)
                    throw new Exception($"Charactor with Id `{id}` not found");
                post.UpVote += 1;
                await _context.SaveChangesAsync();

                servicesResponse.Data = _mapper.Map<GetPostDto>(post);

            } catch(Exception ex) {
                servicesResponse.Success = false;
                servicesResponse.Message = ex.Message;
            }
            return servicesResponse;

        }

        public async Task<ServiceResponse<GetPostDto>> UpdatePost(UpdatePostDto updatePost)
        {
            var servicesResponse = new ServiceResponse<GetPostDto>();

            try
            {
                var post = await _context.Posts.FindAsync(updatePost.Id );

                if (post is null)
                    throw new Exception($"Charactor with Id `{updatePost.Id}` not found");
                post.Title = updatePost.Title;
                post.Content = updatePost.Content;
                post.UpVote = updatePost.UpVote;
                post.DownVote = updatePost.DownVote;
                await _context.SaveChangesAsync();

                servicesResponse.Data = _mapper.Map<GetPostDto>(post);
            }
            catch (Exception ex)
            {   
                servicesResponse.Success = false;
                servicesResponse.Message = ex.Message;
            }
            return servicesResponse;
        }


    }
}