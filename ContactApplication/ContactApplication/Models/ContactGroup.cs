using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ContactApplication.Models;

public class ContactGroup
{
    // Primary key property
    [Key]
    public int ContactGroupId;
    // Foreign key property
    [ForeignKey("Group")]
    public int GroupId { get; set; }
    // Foreign key property
    [ForeignKey("Contact")]
    public int ContactId { get; set; }
    // Navigation properties
    public Group Group { get; set; }
    public Contact Contact { get; set; }

}