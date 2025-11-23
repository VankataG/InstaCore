using InstaCore.Core.Contracts.Repos;
using InstaCore.Core.Dtos.Posts;
using InstaCore.Core.Exceptions;
using InstaCore.Core.Mapping;
using InstaCore.Core.Models;
using InstaCore.Core.Services.Contracts;

namespace InstaCore.Core.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository postRepository;

        private readonly IUserRepository userRepository;

        private readonly IFollowRepository followRepository;

        public PostService(IPostRepository postRepository, IUserRepository userRepository, IFollowRepository followRepository)
        {
            this.postRepository = postRepository;
            this.userRepository = userRepository;
            this.followRepository = followRepository;
        }



        public async Task<PostResponse> CreateAsync(Guid userId, CreatePostRequest request)
        {
            if (string.IsNullOrEmpty(request.Caption))
                throw new NotFoundException("Caption cannot be empty.");
            

            User? user = await userRepository.GetByIdAsync(userId);

            if (user == null)
                throw new NotFoundException("User not found.");

            Post newPost = new Post()
            {
                UserId = userId,
                Caption = request.Caption.Trim(),
                ImageUrl = request.ImageUrl,
            };

            await postRepository.AddAsync(newPost);

            return PostMapper.ToResponse(newPost, user);
        }

        public async Task<PostResponse> GetByIdAsync(Guid postId)
        {
            Post? post = await postRepository.GetByIdAsync(postId);

            if (post == null)
                throw new NotFoundException("Post not found.");

            User user = post.User;
            return PostMapper.ToResponse(post, user);
        }

        public async Task<IReadOnlyList<PostResponse>> GetByUserAsync(string username, int page, int pageSize)
        {
            this.CheckPageAndPageSize(page, pageSize);

            User? user = await userRepository.GetByUsernameAsync(username);
            if (user is null)
                throw new NotFoundException("User not found.");

            int skip = (page - 1) * pageSize;
            int take = pageSize;

            IReadOnlyList<Post> posts = await postRepository.GetByUserAsync(user.Id, skip, take);

            return posts.Select(p => PostMapper.ToResponse(p, user)).ToList().AsReadOnly();
        }

        public async Task<IReadOnlyList<PostResponse>> GetFeedAsync(Guid userId, int page, int pageSize)
        {
            User? user = await userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new NotFoundException("User not found.");

            this.CheckPageAndPageSize(page, pageSize);

            List<Guid> authorIds = new List<Guid>(await followRepository.GetFolloweeIdsAsync(userId));

            if(!authorIds.Contains(userId))
                authorIds.Add(userId);

            if (authorIds.Count == 0)
                return new List<PostResponse>();

            int skip = (page - 1) * pageSize;
            int take = pageSize;

            IReadOnlyList<Post> feed = await postRepository.GetFeedAsync(authorIds, skip, take);

            return feed.Select(p => PostMapper.ToResponse(p, user)).ToList().AsReadOnly();
        }

        private void CheckPageAndPageSize(int page, int pageSize)
        {
            if (page < 1)
                throw new BadRequestException("Page must be at least 1.");

            if (pageSize <= 0 || pageSize > 50)
                throw new BadRequestException("PageSize must be between 1 and 50.");
        }
    }
}
