using System.ComponentModel.DataAnnotations;

namespace PbxApiControl.DTOs.Contact;

public class FindContactDto
{
    [Required]
    public string Number { get; init; }

    [Required]
    public int MinLength { get; init; }

}