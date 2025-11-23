using InstaCore.Core.Contracts.Repos;
using InstaCore.Core.Dtos.Comments;
using InstaCore.Core.Exceptions;
using InstaCore.Core.Models;
using InstaCore.Core.Services.Contracts;

namespace InstaCore.Core.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository commentRepository;

        private readonly IPostRepository postRepository;

        private readonly IUserRepository userRepository;

        public CommentService(ICommentRepository commentRepository, IPostRepository postRepository, IUserRepository userRepository)
        {
            this.commentRepository = commentRepository;
            this.postRepository = postRepository;
            this.userRepository = userRepository;
        }


        public async Task<CreateCommentResponse> AddCommentAsync(Guid userId, Guid postId, CreateCommentRequest request)
        {
            if (string.IsNullOrEmpty(request.Text))
                throw new NotFoundException("You can't add empty comment");

            User? user = await userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new NotFoundException("User not found.");

            if (await postRepository.GetByIdAsync(postId) == null)
                throw new NotFoundException("Post not found.");

            Comment newComment = new Comment()
            {
                UserId = userId,
                PostId = postId,
                Text = request.Text.Trim(),
            };

            await commentRepository.AddAsync(newComment);

            return new CreateCommentResponse()
            {
                CommentId = newComment.Id,
                PostId = newComment.PostId,
                Username = user.Username,
                Text = newComment.Text,
            };
        }

        public async Task DeleteCommentAsync(Guid userId, Guid commentId)
        {
            Comment? comment = await commentRepository.GetByIdAsync(commentId);

            if (comment == null)
                throw new NotFoundException("Comment not found.");

            if (comment.UserId != userId)
                throw new UnauthorizedException("You are not authorized to delete this comment.");

            await commentRepository.DeleteAsync(comment);
        }

        public async Task<IEnumerable<CommentResponse>> ViewAllCommentsAsync(Guid postId)
        {
            if (await postRepository.GetByIdAsync(postId) == null)
                throw new NotImplementedException("Post not found.");

            IEnumerable<Comment> comments = await commentRepository.GetAllCommentsAsync(postId);
            if (comments.Count() <= 0)
                throw new NotFoundException("Post don't have any comments.");

            List<CommentResponse> responseList = new List<CommentResponse>();

            foreach (Comment comment in comments)
            {
                CommentResponse response = new CommentResponse()
                {
                    User = comment.User.Username,
                    Text = comment.Text,
                };

                responseList.Add(response);
            }
            return responseList;
        }
    }
}
