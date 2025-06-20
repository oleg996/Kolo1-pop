using System.ComponentModel.DataAnnotations;

namespace Tutorial9.DTO;

public class NewProgect
{
    public required int projectId { get; set; }

    [MaxLength(200)]

    public required string objective { get; set; }

    public required DateOnly startDate { get; set; }


     public required DateOnly? endDate { get; set; }


    
    


}