using System;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SuitAlterationManager.Domain.SystemManagement.Services.Interfaces;
using SuitAlterationManager.Infrastructure.EF;

namespace PaymentFunctionApp
{
    public class OrderPaidFunction
    {
        private readonly ILogger logger;
        private readonly IAlterationService alterationService;
        private readonly DbContext context;

        public OrderPaidFunction(ILoggerFactory loggerFactory,IAlterationService alterationService, DbContext context)
        {
            this.logger = loggerFactory.CreateLogger<OrderPaidFunction>();
            this.alterationService = alterationService;
            this.context = context;
        }

        [Function("PaymentReceived")]
        public async Task RunAsync([ServiceBusTrigger("OrderPaid", "PaymentReceived", Connection = "ServiceBusConnectionString")] Guid idAlteration)
        {
            logger.LogInformation($"C# PaymentReceived function processed message: {idAlteration}");
            await this.context.Execute(async () =>
            {
                await alterationService.PayAlterationAsync(idAlteration);
            });
        }
    }
}
