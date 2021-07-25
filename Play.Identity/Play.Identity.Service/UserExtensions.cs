using Play.Identity.Service.Entities;

namespace Play.Identity.Service
{
    public static class UserExtensions
    {
        public static UserDto AsDto(this AppUser user)
        {
            return new UserDto(user.Id, user.UserName, user.Email, user.Balance, user.CreatedOn);
        }
    }
}