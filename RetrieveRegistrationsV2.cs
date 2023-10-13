using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Numerics;
using Azure.Data.Tables;
using Azure;
using System.Collections.Generic;
using MCT.Models;
using System.Security.Cryptography;

namespace MCT.Function
{
    public static class RetrieveRegistrationsV2
    {
        [FunctionName("RetrieveRegistrationsV2")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "V2/Registrations")] HttpRequest req,
            ILogger log)
        {
            string connectionString = Environment.GetEnvironmentVariable("TableStorage");
            string partitionKey = "registrations";

            var tableClient = new TableClient(connectionString, partitionKey);
            Pageable<TableEntity> queryRegistrations;
            queryRegistrations = tableClient.Query<TableEntity>(filter: $"PartitionKey eq '(partitionKey)'");

            var returnValue = new List<Registration>();

            foreach (var registration in queryRegistrations)
            {
                returnValue.Add(new Registration()
                {
                    Age = int.Parse(registration["age"].ToString()),
                    Email = registration["email"].ToString(),
                    LastName = registration["lastname"].ToString(),
                    FirstName = registration["firstname"].ToString(),
                    RegistrationId = registration["registrationid"].ToString(),
                    Zipcode = registration["zipcode"].ToString()
                });
            }

            return new OkObjectResult(returnValue);
        }
    }
}
