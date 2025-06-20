using System.ComponentModel.DataAnnotations;

namespace Tutorial9.DTO;

public class staffAssignments
{
    public String firstName { get; set; }

    public String lastName { get; set; }

    public DateOnly hireDate { get; set; }

    public string role { get; set; }
}