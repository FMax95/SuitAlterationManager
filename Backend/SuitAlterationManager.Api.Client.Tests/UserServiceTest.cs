using NSubstitute;
using SuitAlterationManager.Api.Client.Tests.Builder;
using SuitAlterationManager.Domain.Base.Validation;
using SuitAlterationManager.Domain.SystemManagement;
using SuitAlterationManager.Domain.SystemManagement.DTO;
using SuitAlterationManager.Domain.SystemManagement.Repositories;
using SuitAlterationManager.Domain.SystemManagement.ValueObjects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using ApplicationService = SuitAlterationManager.Api.Client.SystemManagement.Services;
using DomainService = SuitAlterationManager.Domain.SystemManagement.Services;

namespace SuitAlterationManager.Api.Client.Tests
{
    public class UserServiceTest
    {
        private IUserRepository userRepository;
        private DomainService.Interfaces.IUserService userService;
        private ApplicationService.Interfaces.IUserService userClientService;

        public UserServiceTest()
        {
            this.userRepository = Substitute.For<IUserRepository>();
            this.userService = new DomainService.UserService(userRepository);
            this.userClientService = new ApplicationService.UserService(userService);
        }

        [Theory(DisplayName = "Create New Client User")]
        [InlineData("a@b.it", "password")]
        public async void CreateClientUser(string email, string password)
        {
            IEnumerable<Guid> groups = new List<Guid>() { new Guid(Group.VISITOR_ID) };
            UserCreatedDTO user = await userClientService.CreateUser(email, password);

            Assert.Equal(email, user.Email);
            Assert.NotEmpty(user.Password);
            Assert.True(user.IsEnabled);
            Assert.Single(user.Groups);
            Assert.Equal(user.Groups[0].ToString(), Group.VISITOR_ID.ToLower());
            Assert.Equal(groups, user.Groups);
        }

        [Theory(DisplayName = "Create New Client User With invalid Email")]
        [InlineData("mailnonvalida", "password")]
        public void CreateClientUserWithInvalidMail(string email, string password)
        {
            Task act() => userClientService.CreateUser(email, password);

            var expectedException = new DomainException(ErrorCodes.InvalidUser);
            var thrownException = Assert.ThrowsAsync<DomainException>(act);
            Assert.Equal(expectedException.Code, thrownException.Result.Code);
        }

        [Theory(DisplayName = "Create New Client User With a Mail already in use")]
        [InlineData("a@b.it", "password")]
        public void CreateClientUserWithMailAlreadyUsed(string email, string password)
        {
            userRepository.ExistsWithEmailAsync(email).Returns(true);
            Task act() => userClientService.CreateUser(email, password);

            var expectedException = new DomainException(ErrorCodes.InvalidUser);
            var thrownException = Assert.ThrowsAsync<DomainException>(act);
            Assert.Equal(expectedException.Code, thrownException.Result.Code);
        }

        [Theory(DisplayName = "Update Client User")]
        [InlineData("nuovamail@mail.it")]
        public void UpdateClientUser(string email)
        {
            UserID userID = new UserID(Guid.NewGuid());
            User user = new UserBuilder();
            userRepository.GetAsync(userID).Returns(user);
            userRepository.ExistsWithEmailAsync(email).Returns(false);

            userClientService.UpdateUser(userID, email, true);

            Assert.Equal(email, user.Email);
        }

        [Theory(DisplayName = "Update Client User With invalid Email")]
        [InlineData("mailnonvalida")]
        public void UpdateClientUserWithInvalidEmail(string email)
        {
            List<Guid> groups = new List<Guid>() { Guid.NewGuid() };
            UserID userID = new UserID(Guid.NewGuid());
            User user = new UserBuilder();
            userRepository.GetAsync(userID).Returns(user);
            userRepository.ExistsWithEmailAsync(email).Returns(false);

            Task act() => userClientService.UpdateUser(userID, email, true);

            var expectedException = new DomainException(ErrorCodes.InvalidUser);
            var thrownException = Assert.ThrowsAsync<DomainException>(act);
            Assert.Equal(expectedException.Code, thrownException.Result.Code);
        }
    }
}
