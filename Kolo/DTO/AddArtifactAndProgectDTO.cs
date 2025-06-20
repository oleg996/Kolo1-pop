using System.ComponentModel.DataAnnotations;

namespace Tutorial9.DTO;

public class NewArtifactProgect
{
    public required NewArtifact artifact { get; set; }
    
    public required NewProgect project { get; set; }

}