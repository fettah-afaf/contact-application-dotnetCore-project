using System.ComponentModel.DataAnnotations.Schema;
namespace ContactApplication.Models;

public class Contact
{
    // Primary key property
    public int ContactId { get; set; }
    // Column properties
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    public string Email { get; set; }
    public string Notes { get; set; }
      // Foreign key property
        [ForeignKey("User")]
        public String UserId { get; set; }
    // Navigation property
    public User User { get; set; }

}