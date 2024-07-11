using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models;

[Table("Todos", Schema = "dbo")]
public class TodoModel
{
    [Key]
    public Guid Id { get; set; }
    public string Author { get; set; }
    public DateTime DateEmmited { get; set; }

    public virtual ICollection<FooModel> Foos { get; set; }
}
