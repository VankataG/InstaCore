using InstaCore.Core.Contracts;
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

        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }


        public async Task<UserResponse> GetByUsernameAsync(string username)
        {
            User? user = await userRepository.GetByUsernameAsync(username);

            if (user == null)
            {
                throw new NotFoundException("User not found");
            }

            return UserMapper.ToResponse(user);
        }

        public async Task<UserResponse> GetMeAsync(Guid userId)
        {
            User? myProfile = await userRepository.GetByIdAsync(userId);

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
    }
}
