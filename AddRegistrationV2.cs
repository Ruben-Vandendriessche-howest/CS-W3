using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using MCT.Models;
using Azure.Data.Tables;

namespace MCT.Function
{
    public static class AddRegistrationV2
    {
        [FunctionName("AddRegistrationV2")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "V2/registrations")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string connectionString = Environment.GetEnvironmentVariable("TableStorage");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var newRegistration = JsonConvert.DeserializeObject<Registration>(requestBody);

            var tableClient = new TableClient(connectionString, "registrations");
            string partitionKey = "registrations";
            string rowKey = Guid.NewGuid().ToString();

            var newEntity = new TableEntity(partitionKey, rowKey){
                {"age", newRegistration.Age},
                {"email", newRegistration.Email},
                {"firstname", newRegistration.FirstName},
                {"isfirsttimer", newRegistration.IsFirstTimer},
                {"lastname", newRegistration.LastName},
                {"zipcode", newRegistration.Zipcode},
                {"registrationid", rowKey}

            };

            await tableClient.AddEntityAsync(newEntity);

            newRegistration.RegistrationId = rowKey;

            return new OkObjectResult(newRegistration);
        }
    }
}
