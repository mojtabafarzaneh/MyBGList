using System.ComponentModel.DataAnnotations;

namespace MyBGListApi.DTO;

public class MechanicDTO
{
    [Required]
    public int Id { get; set; }

    public string? Name { get; set; }
    
}