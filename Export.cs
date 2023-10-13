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
using Azure.Storage.Blobs;

namespace MCT.Function
{
    public class Export
    {
        [FunctionName("Export")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "V2/registrations/exports")] HttpRequest req,
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

            var fileName = WriteCsv(returnValue);
            Upload(fileName);

            return new OkObjectResult(returnValue);
        }

        private void Upload(string fileName)
        {
            string connectionString = Environment.GetEnvironmentVariable("TableStorage");
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

            string containerName = "csv";
            BlobContainerClient bc = blobServiceClient.GetBlobContainerClient(containerName);
            bc.CreateIfNotExists();

            BlobClient bb = bc.GetBlobClient(fileName);
            bb.Upload(fileName);
        }
        private string WriteCsv(List<Registration> registrations)
        {
            string header = "RegistrationId, FirstName, LastName, Age, Zipcode, Email, IsFirstTimer";
            string csv = header + Environment.NewLine;

            foreach (var registration in registrations)
            {
                csv += $"{registration.RegistrationId}, {registration.FirstName}, {registration.LastName}, {registration.Age}, {registration.Zipcode}, {registration.Email}, {registration.IsFirstTimer}";
            }

            string fileName = $"registrations-{DateTime.Now.ToString("yyyyMMddHHmmss")}.csv";
            File.WriteAllText(fileName, csv);

            return fileName;
        }
    }
}
