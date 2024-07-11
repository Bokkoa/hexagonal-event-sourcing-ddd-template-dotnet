using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models;
[Table("Bar", Schema = "dbo")]
public class FooModel
{
    [Key]
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string Email { get; set; }

}
