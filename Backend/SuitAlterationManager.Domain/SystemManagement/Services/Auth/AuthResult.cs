using System;
using System.Collections.Generic;

namespace SuitAlterationManager.Domain.SystemManagement.Services.Auth
{
    public class AuthResult
    {
        public Guid Id { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string Image { get; }
        public string Email { get; }
        public string Token { get; }
        public string RefreshToken { get; }
        public List<Group> Groups { get; }

        public AuthResult(User user, string token, string refreshToken, List<Group> groups = null)
        {
            Id = user.Id.Value;
            FirstName = user.UserInformation?.FirstName;
            LastName = user.UserInformation?.LastName;
            Image = user.UserInformation?.Image;
            Email = user.Email;
            Token = token;
            RefreshToken = refreshToken;
            if (groups != null)
                Groups = groups;
        }
    }
}
