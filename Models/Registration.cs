using System.Text.Json;
using Newtonsoft.Json.Serialization;

namespace MCT.Models;

public class Registration
{
 public string RegistrationId { get; set; }
 public string LastName { get; set; }
 public string FirstName { get; set; }
 public string Email { get; set; }
 public string Zipcode { get; set; }
 public int Age { get; set; }
 public bool IsFirstTimer { get; set; }

 // public Registration(string registrationId, string lastName, string firstName, string email, string zipcode, int age, bool isFirstTimer)
 // {
 //  RegistrationId = registrationId;
 //  LastName = lastName;
 //  FirstName = firstName;
 //  Email = email;
 //  Zipcode = zipcode;
 //  Age = age;
 //  IsFirstTimer = isFirstTimer;
 // }
}
