using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Collections.Generic;
using MCT.Models;
using System.Data;

namespace MCT.Function
{
    public static class AddRegistrations
    {
        [FunctionName("AddRegistrations")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "v1/registrations")] HttpRequest req,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var registration = JsonConvert.DeserializeObject<Registration>(requestBody);

            string ConnectionString = Environment.GetEnvironmentVariable("ConnectionString");
            SqlConnection sqlConnection = new SqlConnection(ConnectionString);
            await sqlConnection.OpenAsync();

            registration.RegistrationId = Guid.NewGuid().ToString();

            SqlCommand sqlCommand = new SqlCommand("insert into Registrations (RegistrationId, Lastname, firstname, email, zipcode, age, isfirsttimer) values (@RegistrationId, @Lastname, @firstname, @email, @zipcode, @age, @isfirsttimer)", sqlConnection);
            sqlCommand.Parameters.AddWithValue("@RegistrationId", registration.RegistrationId);
            sqlCommand.Parameters.AddWithValue("@Lastname", registration.LastName);
            sqlCommand.Parameters.AddWithValue("@firstname", registration.FirstName);
            sqlCommand.Parameters.AddWithValue("@email", registration.Email);
            sqlCommand.Parameters.AddWithValue("@zipcode", registration.Zipcode);
            sqlCommand.Parameters.AddWithValue("@age", registration.Age);
            sqlCommand.Parameters.AddWithValue("@isfirsttimer", registration.IsFirstTimer);

            await sqlCommand.ExecuteNonQueryAsync();

            return new OkObjectResult("");
        }
    }
}
