using InstaCore.Core.Contracts.Repos;
using InstaCore.Core.Dtos.Likes;
using InstaCore.Core.Exceptions;
using InstaCore.Core.Mapping;
using InstaCore.Core.Models;
using InstaCore.Core.Services.Contracts;

namespace InstaCore.Core.Services
{
    public class LikeService : ILikeService
    {
        private readonly IPostRepository postRepository;

        private readonly IUserRepository userRepository;

        private readonly ILikeRepository likeRepository;

        public LikeService(IPostRepository postRepository,IUserRepository userRepository, ILikeRepository likeRepository)
        {
            this.postRepository = postRepository;
            this.userRepository = userRepository;
            this.likeRepository = likeRepository;
        }


        public async Task<LikeResponse> LikeAsync(Guid userId, Guid postId)
        {
            Post? post = await postRepository.GetByIdAsync(postId);
            if(post == null) 
                throw new NotFoundException("Post not found.");

            User? user = await userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new NotFoundException("User not found.");

            Like? existingLike = await likeRepository.GetByUserAndPostAsync(userId, postId);
            if (existingLike != null)
                throw new ConflictException("You already liked this post.");

            Like newLike = new Like()
            { 
                PostId = postId,
                UserId = userId
            };

            await likeRepository.AddAsync(newLike);

            int totalLikes = await likeRepository.GetTotalLikesCountAsync(postId);

            return LikeMapper.ToResponse(newLike, postId, user, totalLikes);
        }

        public async Task<LikeResponse> UnlikeAsync(Guid userId, Guid postId)
        {
            Post? post = await postRepository.GetByIdAsync(postId);
            if (post == null)
                throw new NotFoundException("Post not found.");

            User? user = await userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new NotFoundException("User not found.");

            Like? like = await likeRepository.GetByUserAndPostAsync(userId, postId);
            if (like == null)
                throw new ConflictException("You haven't liked this post.");


            await likeRepository.DeleteAsync(like);

            int totalLikes = await likeRepository.GetTotalLikesCountAsync(postId);

            return LikeMapper.ToResponse(postId, user, totalLikes);
        }
    }
}
