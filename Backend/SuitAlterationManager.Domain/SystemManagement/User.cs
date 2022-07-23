using SuitAlterationManager.Domain.SystemManagement.ValueObjects;
using SuitAlterationManager.Domain.Base.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using SuitAlterationManager.Domain.Base.Validation;
using BC = BCrypt.Net.BCrypt;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace SuitAlterationManager.Domain.SystemManagement
{
    public class User : AggregateRoot<UserID>
    {
        public string Email { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsDeleted { get; set; }
        public string Password { get; set; }
        public DateTimeOffset UpdateDate { get; set; }
        public string ResetToken { get; protected set; }
        public DateTimeOffset? ResetTokenExpirationDate { get; protected set; }
        public DateTimeOffset? PasswordResetDate { get; protected set; }
        public virtual UserInformation UserInformation { get; set; }

        private readonly List<UserGroup> groups = new List<UserGroup>();
        public virtual IReadOnlyList<UserGroup> Groups => groups.AsReadOnly();

        private readonly List<RefreshToken> refreshTokens = new List<RefreshToken>();
        public virtual IReadOnlyList<RefreshToken> RefreshTokens => refreshTokens.AsReadOnly();

        public User()
        {
            this.IsEnabled = true;
            this.UpdateDate = DateTime.Now;
            this.UserInformation = new UserInformation();
        }

        public static User Create(string password, string email, IEnumerable<GroupID> groupIDs, DateTime? birthDate = null, string firstName = null, string lastName = null, string image = null)
        {
            var user = new User
            {
                Id = new UserID(Guid.NewGuid()),
                Password = BC.HashPassword(password),
                Email = email,
                UpdateDate = DateTime.Now
            };
            foreach (GroupID group in groupIDs)
                user.AddGroup(group, DateTime.Now);

            user.UserInformation = UserInformation.Create(user, firstName, lastName, birthDate, image);

            return user;
        }

        public void Update(string email,bool isEnabled, IEnumerable<GroupID> groupIDs, DateTime? birthDate = null, string firstName = null, string lastName = null, string image = null)
        {
            Email = email;
            UpdateDate = DateTime.Now;
            UserInformation.Update(firstName, lastName, birthDate, image);
            IsEnabled = isEnabled;
            var groupsToRemove = this.Groups.Select(x => x.IdGroup).Except(groupIDs).ToList();
            foreach (var idGroup in groupsToRemove)
                this.RemoveGroup(idGroup, DateTime.Now);

            var groupsToAdd = groupIDs.Except(this.Groups.Select(x => x.IdGroup)).ToList();
            foreach (GroupID group in groupsToAdd)
                this.AddGroup(group, DateTime.Now);
        }


        public void Disable(DateTimeOffset updateDate)
        {
            IsDeleted = true;
            UpdateDate = updateDate;
        }


        public void AddGroup(GroupID idGroup, DateTimeOffset updateDate)
        {
            groups.Add(UserGroup.Create(Id, idGroup));

            UpdateDate = updateDate;
        }

        public void RemoveGroup(GroupID idGroup, DateTimeOffset updateDate)
        {
            var group = groups.SingleOrDefault(c => c.IdGroup == idGroup)
                ?? throw new DomainException(ErrorCodes.UserGroupDoesNotExist);

            groups.Remove(group);

            UpdateDate = updateDate;
        }

        public RefreshToken GenerateToken(string ipAddress, DateTimeOffset creationDate, short refreshTokenDaysLifetime)
        {
            var refreshToken = RefreshToken.Create(Id, ipAddress, creationDate, refreshTokenDaysLifetime);
            refreshTokens.Add(refreshToken);
            return refreshToken;
        }

        public bool RevokeToken(string token, string ipAddress, DateTimeOffset revocationDate)
        {
            var oldRefreshToken = RefreshTokens.Single(x => x.Token == token);

            if (!oldRefreshToken.IsActive)
                return false;

            oldRefreshToken.Revoke(null, ipAddress, revocationDate);

            return true;
        }

        public RefreshToken RefreshAuthToken(string token, string ipAddress, DateTimeOffset refreshDate, short refreshTokenDaysLifetime)
        {
            var oldRefreshToken = RefreshTokens.Single(x => x.Token == token);

            if (!oldRefreshToken.IsActive)
                return null;

            var newRefreshToken = GenerateToken(ipAddress, refreshDate, refreshTokenDaysLifetime);
            oldRefreshToken.Revoke(newRefreshToken.Token, ipAddress, refreshDate);
            refreshTokens.Add(newRefreshToken);

            return newRefreshToken;
        }

        public void RemoveOldTokens(short refreshTokenDaysTTL)
        {
            refreshTokens.RemoveAll(x =>
                !x.IsActive &&
                x.CreationDate.AddDays(refreshTokenDaysTTL) <= DateTimeOffset.UtcNow);
        }

        public string GenerateResetToken(DateTimeOffset creationDate, int resetTokenMinutesLifetime)
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[64];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            var resetToken = Convert.ToBase64String(randomBytes);

            var rgx = new Regex("[^a-zA-Z0-9]");
            resetToken = rgx.Replace(resetToken, "");

            ResetToken = resetToken;
            ResetTokenExpirationDate = creationDate.AddMinutes(resetTokenMinutesLifetime);

            return resetToken;
        }

        public void ResetPassword(string newPassword)
        {
            Password = BC.HashPassword(newPassword);
            PasswordResetDate = DateTimeOffset.UtcNow;
            ResetToken = null;
            ResetTokenExpirationDate = null;
        }

    }
}
