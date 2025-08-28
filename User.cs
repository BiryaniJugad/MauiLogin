using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiLogin;

public class User
{
    public string Username { get; set; }      // Unique identifier (can still be used for login)
    public string FirstName { get; set; }     // User's given name
    public string LastName { get; set; }      // User's surname
    public string Email { get; set; }         // User's email (also valid login field)
    public string Password { get; set; }      // Plaintext for now (should be hashed later)
    public string Gender { get; set; }        // Male, Female, Other
    public string Address { get; set; }       // Physical address
    public string DateOfBirth { get; set; }   // Could be string (dd/MM/yyyy) or DateTime
}