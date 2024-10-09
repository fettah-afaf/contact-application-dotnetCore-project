using System.ComponentModel.DataAnnotations.Schema;
namespace ContactApplication.Models;

public class Group
{
    // Primary key property
    public int GroupId { get; set; }


    // Column properties
    public string GroupName { get; set; }
    // Foreign key property
    [ForeignKey("User")]
    public String UserId { get; set; }
    // Navigation property
    public User User { get; set; }



   
  
}