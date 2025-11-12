using InstaCore.Core.Contracts;
using InstaCore.Core.Dtos.Users;
using InstaCore.Core.Exceptions;
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
                throw new ConflictException("User not found");
            }

            return new UserResponse()
            { 
                Id = user!.Id,
                Username = user.Username,
                Bio = user.Bio,
                AvatarUrl = user.AvatarUrl
            };
        }

        public async Task<UserResponse> GetMeAsync(string userId)
        {
            User? myProfile = await userRepository.GetByIdAsync(userId);

            if (myProfile == null)
            {
                throw new ConflictException("User not found");
            }

            return new UserResponse()
            {
                Id = myProfile!.Id,
                Username = myProfile.Username,
                Bio = myProfile.Bio,
                AvatarUrl = myProfile.AvatarUrl
            };
        }

        public async Task<UserResponse> UpdateProfileAsync(string userId, UpdateProfileRequest request)
        {
            User? myProfile = await userRepository.GetByIdAsync(userId);

            if (myProfile == null)
            {
                throw new ConflictException("User not found");
            }

            myProfile.Bio = request.Bio;
            myProfile.AvatarUrl = request.AvatarUrl;
            await userRepository.UpdateAsync(myProfile);


            return new UserResponse() 
            { 
                Id = myProfile!.Id, 
                Username = myProfile.Username, 
                Bio = myProfile.Bio, 
                AvatarUrl = myProfile.AvatarUrl
            };
        }
    }
}
