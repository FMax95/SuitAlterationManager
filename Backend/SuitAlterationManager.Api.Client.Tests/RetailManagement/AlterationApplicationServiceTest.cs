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
using SuitAlterationManager.Api.Client.AlterationManagement.Queries;
using SuitAlterationManager.Domain.SystemManagement.Services.Interfaces;
using SuitAlterationManager.Api.Client.RetailManagement.Services;
using SuitAlterationManager.Infrastructure.MessageDispatchers;

namespace SuitAlterationManager.Api.Client.Tests
{
    [TestClass]
    public class AlterationApplicationServiceTest
    {
        private readonly IAlterationQueries alterationQueries= Substitute.For<IAlterationQueries>();
        private readonly IAlterationService alterationService = Substitute.For<IAlterationService>();
        private readonly IMessageDispatcherService messageDispatcherService = Substitute.For<IMessageDispatcherService>();

        [TestMethod]
        public async Task FinishAlteration()
        {
            var idAlteration = Guid.NewGuid();
            alterationService.FinishAlterationAsync(idAlteration).Returns(Task.CompletedTask);
            alterationQueries.FindAlterationMailAsync(idAlteration).Returns("customer@mail.it");
            messageDispatcherService.SendMessageAsync(Arg.Any<string>(), Arg.Any<string>()).Returns(Task.CompletedTask);

            var alterationApplicationService = new AlterationApplicationService(alterationService, alterationQueries, messageDispatcherService);
            await alterationApplicationService.FinishAlteration(idAlteration);
            _ = alterationService.Received(1).FinishAlterationAsync(idAlteration);
            _ = messageDispatcherService.Received(1).SendMessageAsync("AlterationFinished", Arg.Any<string>());
        }

    }
}
