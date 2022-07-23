using NSubstitute;
using SuitAlterationManager.Api.CMS.SystemManagement.Queries;
using SuitAlterationManager.Api.CMS.SystemManagement.Service;
using SuitAlterationManager.Api.CMS.SystemManagement.Service.Interfaces;
using SuitAlterationManager.Domain.SystemManagement.ValueObjects;
using System;
using System.Collections.Generic;
using Xunit;

namespace SuitAlterationManager.Domain.Tests
{
    public class PermissionServiceTest
    {
        private IPermissionService permissionService;
        private IPermissionQueries permissionQueries;
        public PermissionServiceTest()
        {
            this.permissionQueries = Substitute.For<IPermissionQueries>();
            this.permissionService = new PermissionService(permissionQueries);
        }

        [Theory(DisplayName = "Validate Permission")]
        [InlineData("Read", "User")]
        public async void ValidatePermission(string actionName, string contextName)
        {
            UserID userId = new UserID(Guid.NewGuid());
            Dictionary<string, List<string>> userPermissions = new Dictionary<string, List<string>>();
            userPermissions.Add("User", new List<string>() { "Read", "Create", "Update", "Delete" });
            permissionQueries.GetUserPermissionsAsync(userId).Returns(userPermissions);

            bool result = await permissionService.ValidatePermission(userId, actionName, contextName);

            await permissionQueries.Received().GetUserPermissionsAsync(userId);
            Assert.True(result);
        }

        [Theory(DisplayName = "Validate Permission Without Requested Context")]
        [InlineData("Read", "User")]
        public async void ValidatePermissionWithoutRequestedContext(string actionName, string contextName)
        {
            UserID userId = new UserID(Guid.NewGuid());
            Dictionary<string, List<string>> userPermissions = new Dictionary<string, List<string>>();
            userPermissions.Add("Order", new List<string>() { "Read", "Create", "Update", "Delete" });
            permissionQueries.GetUserPermissionsAsync(userId).Returns(userPermissions);

            bool result = await permissionService.ValidatePermission(userId, actionName, contextName);

            await permissionQueries.Received().GetUserPermissionsAsync(userId);
            Assert.False(result);
        }

        [Theory(DisplayName = "Validate Permission Without Requested Action")]
        [InlineData("Create", "User")]
        public async void ValidatePermissionWithoutRequestedAction(string actionName, string contextName)
        {
            UserID userId = new UserID(Guid.NewGuid());
            Dictionary<string, List<string>> userPermissions = new Dictionary<string, List<string>>();
            userPermissions.Add("User", new List<string>() { "Read", "Update" });
            permissionQueries.GetUserPermissionsAsync(userId).Returns(userPermissions);

            bool result = await permissionService.ValidatePermission(userId, actionName, contextName);

            await permissionQueries.Received().GetUserPermissionsAsync(userId);
            Assert.False(result);
        }

        [Theory(DisplayName = "Validate Permission With Invalid User")]
        [InlineData("Read", "User")]
        public async void ValidatePermissionWithInvalidUser(string actionName, string contextName)
        {
            UserID userId = new UserID(Guid.NewGuid());
            Dictionary<string, List<string>> userPermissions = new Dictionary<string, List<string>>();
            permissionQueries.GetUserPermissionsAsync(userId).Returns(userPermissions);

            bool result = await permissionService.ValidatePermission(userId, actionName, contextName);

            await permissionQueries.Received().GetUserPermissionsAsync(userId);
            Assert.False(result);
        }

        [Theory(DisplayName = "Validate Permission Without User")]
        [InlineData("Read", "User")]
        public async void ValidatePermissionWithoutUser(string actionName, string contextName)
        {
            UserID userId = null;
            Dictionary<string, List<string>> userPermissions = new Dictionary<string, List<string>>();
            permissionQueries.GetUserPermissionsAsync(userId).Returns(userPermissions);

            bool result = await permissionService.ValidatePermission(userId, actionName, contextName);

            await permissionQueries.DidNotReceive().GetUserPermissionsAsync(Arg.Any<UserID>());
            Assert.False(result);
        }

        [Theory(DisplayName = "Validate Permission With Invalid Context")]
        [InlineData("Read", "UserS")]
        public async void ValidatePermissionWithInvalidContext(string actionName, string contextName)
        {
            UserID userId = new UserID(Guid.NewGuid());
            Dictionary<string, List<string>> userPermissions = new Dictionary<string, List<string>>();
            permissionQueries.GetUserPermissionsAsync(userId).Returns(userPermissions);

            bool result = await permissionService.ValidatePermission(userId, actionName, contextName);

            await permissionQueries.Received().GetUserPermissionsAsync(userId);
            Assert.False(result);
        }

        [Theory(DisplayName = "Validate Permission With Invalid Action")]
        [InlineData("ReadAll", "User")]
        public async void ValidatePermissionWithInvalidAction(string actionName, string contextName)
        {
            UserID userId = new UserID(Guid.NewGuid());
            Dictionary<string, List<string>> userPermissions = new Dictionary<string, List<string>>();
            permissionQueries.GetUserPermissionsAsync(userId).Returns(userPermissions);

            bool result = await permissionService.ValidatePermission(userId, actionName, contextName);

            await permissionQueries.Received().GetUserPermissionsAsync(userId);
            Assert.False(result);
        }
    }
}
