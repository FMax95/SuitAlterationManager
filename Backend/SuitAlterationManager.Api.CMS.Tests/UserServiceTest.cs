using NSubstitute;
using SuitAlterationManager.Api.CMS.Tests.Builder;
using SuitAlterationManager.Domain.Base.Validation;
using SuitAlterationManager.Domain.SystemManagement;
using SuitAlterationManager.Domain.SystemManagement.DTO;
using SuitAlterationManager.Domain.SystemManagement.Repositories;
using SuitAlterationManager.Domain.SystemManagement.ValueObjects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using ApplicationService = SuitAlterationManager.Api.CMS.SystemManagement.Services;
using DomainService = SuitAlterationManager.Domain.SystemManagement.Services;

namespace SuitAlterationManager.Api.CMS.Tests
{
    public class UserServiceTest
    {
        private IUserRepository userRepository;
        private DomainService.Interfaces.IUserService userService;
        private ApplicationService.Interfaces.IUserService userCmsService;

        public UserServiceTest()
        {
            this.userRepository = Substitute.For<IUserRepository>();
            this.userService = new DomainService.UserService(userRepository);
            this.userCmsService = new ApplicationService.UserService(userService);
        }

        [Theory(DisplayName = "Create New CMS User")]
        [InlineData("a@b.it", "password")]
        public async void CreateCMSUser(string email, string password)
        {
            List<Guid> groups = new List<Guid>() { Guid.NewGuid() };
            UserCreatedDTO user = await userCmsService.CreateUser(email, password, groups);

            Assert.Equal(email, user.Email);
            Assert.NotEmpty(user.Password);
            Assert.True(user.IsEnabled);
            Assert.Single(user.Groups);
        }

        [Theory(DisplayName = "Create New CMS User With invalid Email")]
        [InlineData("mailnonvalida", "password")]
        public void CreateCMSUserWithInvalidMail(string email, string password)
        {
            List<Guid> groups = new List<Guid>() { Guid.NewGuid() };
            Task act() => userCmsService.CreateUser(email, password, groups);

            var expectedException = new DomainException(ErrorCodes.EmailNotValid);
            var thrownException = Assert.ThrowsAsync<DomainException>(act);
            Assert.Equal(expectedException.Code, thrownException.Result.Code);
        }

        [Theory(DisplayName = "Create New CMS User Without groups")]
        [InlineData("a@b.it", "password")]
        public void CreateCMSUserWithoutGroups(string email, string password)
        {
            List<Guid> groups = new List<Guid>();
            Task act() => userCmsService.CreateUser(email, password, groups);

            var expectedException = new DomainException(ErrorCodes.UserWithoutGroups);
            var thrownException = Assert.ThrowsAsync<DomainException>(act);
            Assert.Equal(expectedException.Code, thrownException.Result.Code);
        }

        [Theory(DisplayName = "Create New CMS User With a Mail already in use")]
        [InlineData("a@b.it", "password")]
        public void CreateCMSUserWithMailAlreadyUsed(string email, string password)
        {
            List<Guid> groups = new List<Guid>() { Guid.NewGuid() };
            userRepository.ExistsWithEmailAsync(email).Returns(true);
            Task act() => userCmsService.CreateUser(email, password, groups);

            var expectedException = new DomainException(ErrorCodes.EmailDuplicated);
            var thrownException = Assert.ThrowsAsync<DomainException>(act);
            Assert.Equal(expectedException.Code, thrownException.Result.Code);
        }

        [Theory(DisplayName = "Update CMS User")]
        [InlineData("nuovamail@mail.it")]
        public void UpdateCMSUser(string email)
        {
            List<Guid> groups = new List<Guid>() { Guid.NewGuid() };
            UserID userID = new UserID(Guid.NewGuid());
            User user = new UserBuilder();
            userRepository.GetAsync(userID).Returns(user);
            userRepository.ExistsWithEmailAsync(email).Returns(false);

            userCmsService.UpdateUser(userID, email, true, groups);

            Assert.Equal(email, user.Email);
        }

        [Theory(DisplayName = "Update CMS User With invalid Email")]
        [InlineData("mailnonvalida")]
        public void UpdateCMSUserWithInvalidEmail(string email)
        {
            List<Guid> groups = new List<Guid>() { Guid.NewGuid() };
            UserID userID = new UserID(Guid.NewGuid());
            User user = new UserBuilder();
            userRepository.GetAsync(userID).Returns(user);
            userRepository.ExistsWithEmailAsync(email).Returns(false);

            Task act() => userCmsService.UpdateUser(userID, email, true, groups);

            var expectedException = new DomainException(ErrorCodes.EmailNotValid);
            var thrownException = Assert.ThrowsAsync<DomainException>(act);
            Assert.Equal(expectedException.Code, thrownException.Result.Code);
        }
    }
}
