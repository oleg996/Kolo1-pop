using System.ComponentModel.DataAnnotations;

namespace Tutorial9.DTO;

public class Progect
{
    public int ProgectID { get; set; }
    public string objective { get; set; }

    public DateOnly startDate { get; set; }

    public DateOnly? endDate { get; set; }

    public Artifact artifact { get; set; }


    public List<staffAssignments> staffAssignments { get; set; }




}