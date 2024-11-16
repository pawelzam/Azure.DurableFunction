using DurableFunction.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.DurableTask;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Logging;

namespace DurableFunction
{
    public static class FunctionOrchestrator
    {
        [Function(nameof(FunctionOrchestrator))]
        public static async Task<Order> RunOrchestrator(
            [OrchestrationTrigger] TaskOrchestrationContext context)
        {
            ILogger logger = context.CreateReplaySafeLogger(nameof(FunctionOrchestrator));
            var order = new Order();
            logger.LogInformation("Processing order {Id}", order.Id);
            
            order = await context.CallActivityAsync<Order>(nameof(ProcessOrder), order);
            order = await context.CallActivityAsync<Order>(nameof(ProcessOrder), order);
            order = await context.CallActivityAsync<Order>(nameof(ProcessOrder), order);

            logger.LogInformation("Final state of order {Id} is {State}", order.Id, order.State);

            return order;
        }

        [Function(nameof(ProcessOrder))]
        public static Order ProcessOrder([ActivityTrigger] Order order, FunctionContext executionContext)
        {
            ILogger logger = executionContext.GetLogger(nameof(ProcessOrder));
            switch (order.State)
            {
                case OrderState.Created:
                    order.State = OrderState.Paid;
                    break;
                case OrderState.Paid:
                    order.State = OrderState.Delivered;
                    break;
                case OrderState.Delivered:
                    order.State = OrderState.Invoiced;
                    break;
            }

            logger.LogInformation("Order {Id} state changed to {State}.", order.Id, order.State);
            return order;
        }

        [Function("Function_HttpStart")]
        public static async Task<HttpResponseData> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req,
            [DurableClient] DurableTaskClient client,
            FunctionContext executionContext)
        {
            ILogger logger = executionContext.GetLogger("Function_HttpStart");

            // Function input comes from the request content.
            string instanceId = await client.ScheduleNewOrchestrationInstanceAsync(
                nameof(FunctionOrchestrator));

            logger.LogInformation("Started orchestration with ID = '{instanceId}'.", instanceId);

            // Returns an HTTP 202 response with an instance management payload.
            // See https://learn.microsoft.com/azure/azure-functions/durable/durable-functions-http-api#start-orchestration
            return await client.CreateCheckStatusResponseAsync(req, instanceId);
        }
    }
}
