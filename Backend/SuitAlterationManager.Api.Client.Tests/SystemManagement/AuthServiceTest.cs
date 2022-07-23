using SuitAlterationManager.Domain.AlterationManagement;
using SuitAlterationManager.Domain.AlterationManagement.Enum;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuitAlterationManager.Domain.Base.Validation;
using SuitAlterationManager.Api.Client.SystemManagement.Queries;
using NSubstitute;
using SuitAlterationManager.Api.Client.SystemManagement.Services.Interfaces;
using NSubstitute.Extensions;
using SuitAlterationManager.Api.Client.SystemManagement.Responses;
using System.Threading.Tasks;
using SuitAlterationManager.Api.Client.SystemManagement.Services;
using System;

namespace SuitAlterationManager.Api.Client.Tests
{
    [TestClass]
    public class LiveSessionApplicationServiceTest
    {
        private readonly IUserQueries userQueries = Substitute.For<IUserQueries>();


        [TestMethod]
        [DataRow("test@mail.it", "psw")]
        public void AuthWrongEmail(string email, string password)
        {
            var authService = Substitute.ForPartsOf<AuthService>(userQueries);

            userQueries.FindUserByEmailAsync(email).Returns(Task.FromResult((UserResponse)null));

            _ = Assert.That.ThrowsWithCodeAsync<ApplicationServiceException>(ApplicationServiceExceptionCode.WrongEmail, () =>
                  authService.Authenticate(email, password));
        }

        [TestMethod]
        [DataRow("test@mail.it", "psw")]
        public void AuthWrongPassword(string email, string password)
        {
            var authService = Substitute.ForPartsOf<AuthService>(userQueries);
            var response = new UserResponse()
            {
                Email = email,
                Password = "a different password",
                Id = Guid.NewGuid()
            };

            authService.Configure().VerifyPassword(response.Password,password).Returns(false);
            userQueries.FindUserByEmailAsync(email).Returns(Task.FromResult(response));

            _ = Assert.That.ThrowsWithCodeAsync<ApplicationServiceException>(ApplicationServiceExceptionCode.WrongPassword, () =>
                  authService.Authenticate(email, password));
        }

        [TestMethod]
        [DataRow("test@mail.it", "psw")]
        public async Task AuthOk(string email, string password)
        {
            var authService = Substitute.ForPartsOf<AuthService>(userQueries);
            var response = new UserResponse()
            {
                Email = email,
                Password = "psw",
                Id = Guid.NewGuid()
            };

            authService.Configure().VerifyPassword(response.Password, password).Returns(true);
            userQueries.FindUserByEmailAsync(email).Returns(Task.FromResult(response));

            var authResponse = await authService.Authenticate(email, password);

            Assert.AreEqual(email, authResponse.Email);
            Assert.IsFalse(string.IsNullOrWhiteSpace(authResponse.Token));
        }
    }
}
