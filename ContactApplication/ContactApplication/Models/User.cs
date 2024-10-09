using Microsoft.AspNetCore.Identity;
namespace ContactApplication.Models;

public class User: IdentityUser
{
    
    public string?  Name { get; set; }
    public string Address{ get; set; }
   
}