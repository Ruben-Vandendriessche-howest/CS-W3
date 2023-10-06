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
            var registration = Jsonconvert.DeserialiseObject

            string ConnectionString = Environment.GetEnvironmentVariable("ConnectionString");
            SqlConnection sqlConnection = new SqlConnection(ConnectionString);
            await sqlConnection.OpenAsync();

            SqlCommand sqlCommand = new SqlCommand("insert into Registrations (Lastname, firstname, email, zipcode, age, isfirsttimer) values (@Lastname, @firstname, @email, @zipcode, @age, @isfirsttimer)", sqlConnection);

            sqlCommand.Parameters.AddWithValue("@Lastname", LastName);
            sqlCommand.Parameters.AddWithValue("@firstname", LastName);
            sqlCommand.Parameters.AddWithValue("@email", LastName);
            sqlCommand.Parameters.AddWithValue("@zipcode", LastName);
            sqlCommand.Parameters.AddWithValue("@age", LastName);
            sqlCommand.Parameters.AddWithValue("@isfirsttimer", LastName);

            return new OkObjectResult("");
        }
    }
}
