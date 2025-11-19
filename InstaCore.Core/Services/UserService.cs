using InstaCore.Core.Contracts.Repos;
using InstaCore.Core.Dtos.Users;
using InstaCore.Core.Exceptions;
using InstaCore.Core.Mapping;
using InstaCore.Core.Models;
using InstaCore.Core.Services.Contracts;

namespace InstaCore.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;

        private readonly IFollowRepository followRepository;

        public UserService(IUserRepository userRepository, IFollowRepository followRepository)
        {
            this.userRepository = userRepository;
            this.followRepository = followRepository;
        }

        
        public async Task<UserResponse> GetByUsernameAsync(string username)
        {
            User? user = await userRepository.GetByUsernameWithFollowsAsync(username);

            if (user == null)
            {
                throw new NotFoundException("User not found");
            }

            return UserMapper.ToResponse(user);
        }

        public async Task<UserResponse> GetMeAsync(Guid userId)
        {
            User? myProfile = await userRepository.GetByIdWithFollowsAsync(userId);

            if (myProfile == null)
            {
                throw new NotFoundException("User not found");
            }

            return UserMapper.ToResponse(myProfile);
        }

        

        public async Task<UserResponse> UpdateProfileAsync(Guid userId, UpdateProfileRequest request)
        {
            User? myProfile = await userRepository.GetByIdAsync(userId);

            if (myProfile == null)
            {
                throw new NotFoundException("User not found");
            }

            myProfile.Bio = request.Bio;
            myProfile.AvatarUrl = request.AvatarUrl;
            await userRepository.UpdateAsync(myProfile);


            return UserMapper.ToResponse(myProfile);
        }

        public async Task<bool> FollowAsync(Guid followerId, string followeeUsername)
        {
            User? followeeUser = await userRepository.GetByUsernameWithFollowsAsync(followeeUsername.Trim());

            if (followeeUser == null)
                throw new NotFoundException("User not found.");

            if (followeeUser.Id == followerId)
                throw new BadRequestException("You cannot follow yourself.");

            if (await followRepository.ExistsAsync(followerId, followeeUser.Id))
                return false; //Already following.

            Follow newFollow = new Follow()
            {
                FollowerId = followerId,
                FolloweeId = followeeUser.Id
            };

            await followRepository.AddAsync(newFollow);
            return true;
        }

        public async Task<bool> UnfollowAsync(Guid followerId, string followeeUsername)
        {
            User? followeeUser = await userRepository.GetByUsernameWithFollowsAsync(followeeUsername.Trim());

            if (followeeUser == null)
                throw new NotFoundException("User not found.");

            if (!await followRepository.ExistsAsync(followerId, followeeUser.Id))
                return false; //Not following this user.

            await followRepository.RemoveAsync(followerId, followeeUser.Id);
            return true;
        }


        
    }
}
