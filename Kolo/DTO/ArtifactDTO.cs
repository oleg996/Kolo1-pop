using System.ComponentModel.DataAnnotations;

namespace Tutorial9.DTO;

public class Artifact
{
    public string name { get; set; }

    public DateOnly originDate { get; set; }

    public Institution institution { get; set; }


}