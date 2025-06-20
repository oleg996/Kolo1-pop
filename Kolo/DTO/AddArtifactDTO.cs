using System.ComponentModel.DataAnnotations;

namespace Tutorial9.DTO;

public class NewArtifact
{
    public required int artifactId { get; set; }

    [MaxLength(100)]

    public required string name { get; set; }

    public required DateOnly originDate { get; set; }

    public required int institutionId { get; set; }
    
    


}