using SuitAlterationManager.Domain.SystemManagement.Services.Interfaces;
using SuitAlterationManager.Domain.Base.Validation;
using SuitAlterationManager.Domain.SystemManagement.Repositories;
using SuitAlterationManager.Domain.SystemManagement.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SuitAlterationManager.Domain.SystemManagement.DTO;

namespace SuitAlterationManager.Domain.SystemManagement.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;

        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private async Task<bool> IsValid(string email, IEnumerable<GroupID> groups, UserID currentId = null)
        {
            if (IsValidEmail(email) == false)
                throw new DomainException(ErrorCodes.EmailNotValid);

            if (await this.userRepository.ExistsWithEmailAsync(email: email,
                                                     differentThan: currentId))
                throw new DomainException(ErrorCodes.EmailDuplicated);

            if (groups.Any() == false)
                throw new DomainException(ErrorCodes.UserWithoutGroups);

            return true;
        }

        public async Task<UserCreatedDTO> CreateUser(string email, string password, IEnumerable<GroupID> groupIDs, DateTime? birthDate = null, string firstName = null, string lastName = null, string image = null)
        {
            User user = null;
            if (await IsValid(email: email,
                              groups: groupIDs))
            {
                user = User.Create(
                    password: password,
                    email: email,
                    groupIDs: groupIDs,
                    birthDate: birthDate,
                    firstName: firstName,
                    lastName: lastName,
                    image: image);

                this.userRepository.Add(user);
            }
            if (user == null)
                return null;
            return new UserCreatedDTO()
            {
                Id = user.Id.Value,
                Email = user.Email,
                Password = user.Password,
                Image = user.UserInformation.Image,
                FirstName = user.UserInformation.FirstName,
                LastName = user.UserInformation.LastName,
                IsEnabled = user.IsEnabled,
                Groups = user.Groups.Select(x => x.IdGroup.Value).ToList()
            };
        }

        public async Task UpdateUser(UserID userId, string email, bool isEnabled, IEnumerable<GroupID> groupIDs, DateTime? birthDate = null, string firstName = null, string lastName = null, string image = null)
        {
            User user = await this.userRepository.GetAsync(userId);
            if (await IsValid(email: email, groups: groupIDs, currentId: userId))
                user.Update(email: email,
                        isEnabled: isEnabled,
                        groupIDs: groupIDs,
                        birthDate: birthDate,
                        firstName: firstName,
                        lastName: lastName,
                        image: image);

            this.userRepository.Update(user);
        }

        public async Task DisableUser(UserID userId)
        {
            User user = await this.userRepository.GetAsync(userId);
            user.Disable(DateTime.Now);
            this.userRepository.Update(user);
        }

    }
}
